using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Controllers;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace TimeProductivityTracking.UnitTest
{
    public class UserInfoesControllerTests
    {
        private readonly Mock<UserManager<IdentityAuthUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<ILogger<UserInfoesController>> _loggerMock;

        public UserInfoesControllerTests()
        {
            _userManagerMock = new Mock<UserManager<IdentityAuthUser>>(
                Mock.Of<IUserStore<IdentityAuthUser>>(),
                null, null, null, null, null, null, null, null);

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(),
                new IRoleValidator<IdentityRole>[0],
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

            _loggerMock = new Mock<ILogger<UserInfoesController>>();
        }

        [Fact]
        public async Task Index_ReturnsViewWithUsers()
        {
            // Arrange: create in-memory db context
            var dbContext = new ProductivitiesContext(
                new DbContextOptionsBuilder<ProductivitiesContext>()
                .UseInMemoryDatabase("UserIndexTest").Options);

            // Add test Rate
            dbContext.Rates.Add(new Rate
            {
                RateID = 1,
                RateName = "Standard",
                HourlyWage = 25.0
            });

            // Add test UserInfo
            dbContext.Users.Add(new UserInfo
            {
                UserId = 1,
                FName = "Test",
                LName = "User",
                Email = "test@example.com",
                RateID = 1
            });

            dbContext.SaveChanges();

            var controller = new UserInfoesController(
                dbContext,
                _roleManagerMock.Object,
                _userManagerMock.Object,
                _loggerMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<UserInfo>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("test@example.com", model[0].Email);
        }
    }
}
