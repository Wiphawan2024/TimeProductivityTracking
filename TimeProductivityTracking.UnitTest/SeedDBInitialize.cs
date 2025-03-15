using Microsoft.AspNetCore.Identity;
using Moq;
using TimeProductivityTracking.web.Areas.Identity.Data;
using Xunit;

namespace TimeProductivityTracking.UnitTest  // ✅ Add a proper namespace
{
    public class SeedDBInitializeTests
    {
        [Fact]
        public async Task InitializeAsync_ShouldCreateRoles_IfTheyDoNotExist()
        {
            // Arrange
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            // Mock RoleExistsAsync to return false (meaning roles do not exist yet)
            mockRoleManager.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Mock CreateAsync to return a successful result
            mockRoleManager.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)))
                .Returns(mockRoleManager.Object);

            // Act
            await SeedDBInitialize.InitializeAsync(serviceProvider.Object);

            // Assert
            mockRoleManager.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Admin")), Times.Once);
            mockRoleManager.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Manager")), Times.Once);
            mockRoleManager.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Contractor")), Times.Once);
            mockRoleManager.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "HR")), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_ShouldNotCreateRoles_IfTheyAlreadyExist()
        {
            // Arrange
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            // Mock RoleExistsAsync to return true (roles already exist)
            mockRoleManager.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)))
                .Returns(mockRoleManager.Object);

            // Act
            await SeedDBInitialize.InitializeAsync(serviceProvider.Object);

            // Assert (Ensure CreateAsync is never called since roles exist)
            mockRoleManager.Verify(r => r.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
        }
    }
}