using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using TimeProductivityTracking.web.Areas.Identity.Data;
using Xunit;

namespace TimeProductivityTracking.UnitTest
{
    public class SeedDBInitializeTests
    {
        [Fact]
        public async Task InitializeAsync_ShouldCreateRoles_IfTheyDoNotExist()
        {
            // Arrange
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object,
                Array.Empty<IRoleValidator<IdentityRole>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

      
            // Roles do not exist
            roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            var userStoreMock = new Mock<IUserStore<IdentityAuthUser>>();
            var userManagerMock = new Mock<UserManager<IdentityAuthUser>>(
                userStoreMock.Object,
                null, // IOptions<IdentityOptions>
                new PasswordHasher<IdentityAuthUser>(),
                Array.Empty<IUserValidator<IdentityAuthUser>>(),
                Array.Empty<IPasswordValidator<IdentityAuthUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null, // IServiceProvider
                new Mock<ILogger<UserManager<IdentityAuthUser>>>().Object);


            // Admin user does not exist
            userManagerMock.Setup(u => u.FindByEmailAsync("admin@domain.com")).ReturnsAsync((IdentityAuthUser)null);
            userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityAuthUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityAuthUser>(), "Admin"))
                           .ReturnsAsync(IdentityResult.Success);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(s => s.GetService(typeof(RoleManager<IdentityRole>)))
                               .Returns(roleManagerMock.Object);
            serviceProviderMock.Setup(s => s.GetService(typeof(UserManager<IdentityAuthUser>)))
                               .Returns(userManagerMock.Object);

            // Act
            await SeedDBInitialize.InitializeAsync(serviceProviderMock.Object);

            // Assert
            roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(x => x.Name == "Admin")), Times.Once);
            roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(x => x.Name == "Manager")), Times.Once);
            roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(x => x.Name == "Contractor")), Times.Once);
            roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(x => x.Name == "HR")), Times.Once);

            userManagerMock.Verify(u => u.CreateAsync(It.Is<IdentityAuthUser>(u => u.Email == "admin@domain.com"), "Admin123!"), Times.Once);
            userManagerMock.Verify(u => u.AddToRoleAsync(It.IsAny<IdentityAuthUser>(), "Admin"), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_ShouldNotCreateRoles_IfTheyExist()
        {
            // Arrange
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(),
                Array.Empty<IRoleValidator<IdentityRole>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

            // Roles already exist
            roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var userManagerMock = new Mock<UserManager<IdentityAuthUser>>(
                Mock.Of<IUserStore<IdentityAuthUser>>(),
                null, null, null, null, null, null, null,
                new Mock<ILogger<UserManager<IdentityAuthUser>>>().Object);

            // Admin user already exists
            userManagerMock.Setup(u => u.FindByEmailAsync("admin@domain.com"))
                           .ReturnsAsync(new IdentityAuthUser());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(s => s.GetService(typeof(RoleManager<IdentityRole>)))
                               .Returns(roleManagerMock.Object);
            serviceProviderMock.Setup(s => s.GetService(typeof(UserManager<IdentityAuthUser>)))
                               .Returns(userManagerMock.Object);

            // Act
            await SeedDBInitialize.InitializeAsync(serviceProviderMock.Object);

            // Assert
            roleManagerMock.Verify(r => r.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            userManagerMock.Verify(u => u.CreateAsync(It.IsAny<IdentityAuthUser>(), It.IsAny<string>()), Times.Never);
            userManagerMock.Verify(u => u.AddToRoleAsync(It.IsAny<IdentityAuthUser>(), It.IsAny<string>()), Times.Never);
        }
    }
}
