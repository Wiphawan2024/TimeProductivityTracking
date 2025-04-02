using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeProductivityTracking.web.Controllers;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class ProductivitiesControllerTests : IDisposable
{
    private readonly ProductivitiesContext _context;
    private readonly ProductivitiesController _controller;

    
    public ProductivitiesControllerTests()
    { 
        _context = SetUpTestDatabase();
        _controller = new ProductivitiesController(_context);

        // Mock User.Identity.Name to avoid null reference issues
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser@sec.ie")
        }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    private ProductivitiesContext SetUpTestDatabase()
    {
        var options = new DbContextOptionsBuilder<ProductivitiesContext>()
            .UseInMemoryDatabase(databaseName: "TestProductivityDB" + System.Guid.NewGuid()) // Unique DB per test
            .Options;

        var context = new ProductivitiesContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task Create_Post_ValidData_ShouldSaveToDatabase_AndRedirectToIndex()
    {
        // Arrange
        var productivityList = new List<Productivity>
        {
            new Productivity { SECName = "SEC 1", County = Counties.Longford, PlannedDays = 5 },
            new Productivity { SECName = "SEC 2", County = Counties.Offaly, PlannedDays = 10 }
        };

        string selectedMonth = "1"; // January

        // Act
        var result = await _controller.Create(selectedMonth, productivityList) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Check if data was saved
        var savedData = await _context.Productivities.ToListAsync();
        Assert.Equal(2, savedData.Count);
        Assert.Equal("January 2025", savedData[0].Monthly);
        Assert.Equal(5, savedData[0].PlannedDays);
        Assert.Equal("SEC 1", savedData[0].SECName);
        Assert.Equal(Counties.Longford, savedData[0].County);
    }

    [Fact]
    public async Task Create_Post_InvalidModel_ShouldReturnRedirectToIndex()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Model is invalid");

        var productivityList = new List<Productivity>(); // Empty data

        // Act
        var result = await _controller.Create("1", productivityList) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Ensure no data was added
        var savedData = await _context.Productivities.ToListAsync();
        Assert.Empty(savedData);
    }

    [Fact]
    public async Task Create_Post_ShouldAssignCorrectMonthBasedOnSelectedMonth()
    {
        // Arrange
        var productivityList = new List<Productivity>
        {
            new Productivity { SECName = "SEC 1", County = Counties.Longford, PlannedDays = 8 }
        };

        string selectedMonth = "3"; // March

        // Act
        await _controller.Create(selectedMonth, productivityList);
        var savedData = await _context.Productivities.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(savedData);
        Assert.Equal("March 2025", savedData.Monthly);
    }

    [Fact]
    public async Task Edit_IdIsNull_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.Edit(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ProductivityNotFound_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.Edit(999); // ID that doesn't exist

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ProductivityExists_ShouldReturnViewWithProductivity()
    {
        // Arrange
        var testProductivity = new Productivity { Id = 1, SECName = "Test SEC", County = Counties.Longford, PlannedDays = 5 };
        _context.Productivities.Add(testProductivity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Productivity>(viewResult.Model);
        Assert.Equal(1, model.Id);
        Assert.Equal("Test SEC", model.SECName);
        Assert.Equal(Counties.Longford, model.County);
    }
}
