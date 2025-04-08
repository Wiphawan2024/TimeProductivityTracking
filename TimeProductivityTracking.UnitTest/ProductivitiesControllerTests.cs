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
using System;

public class ProductivitiesControllerTests : IDisposable
{
    private readonly ProductivitiesContext _context;
    private readonly ProductivitiesController _controller;

    public ProductivitiesControllerTests()
    {
        _context = SetUpTestDatabase();
        _controller = new ProductivitiesController(_context);

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
            .UseInMemoryDatabase(databaseName: "TestProductivityDB" + Guid.NewGuid())
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
    public async Task Index_WithSelectedMonth_ShouldReturnProductivities()
    {
        // Arrange
        var testProductivity = new Productivity
        {
            SECName = "Test SEC",
            County = Counties.Longford,
            PlannedDays = 5,
            Task_P = Tasks.EMPApplicationSubmitted,
            Tasks_A = Tasks.EMPCompleted,
            AchevedDays = 3,
            CountryMentor_P = "Mentor P",
            CountryMentor_A = "Mentor A",
            Monthly = "January 2025",
            Date = DateTime.Now,
            ContractorId = 1,
            UserEmail = "testuser@sec.ie",
            statusApproval = "Waiting"
        };

        _context.Productivities.Add(testProductivity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index("January 2025");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Productivity>>(viewResult.Model);
        var item = Assert.Single(model);
        Assert.Equal("Test SEC", item.SECName);
    }



    [Fact]
    public async Task Create_Post_InvalidModel_ShouldReturnRedirectToIndex()
    {
        _controller.ModelState.AddModelError("Error", "Model is invalid");

        var productivityList = new List<Productivity>();

        var result = await _controller.Create("1","2025", productivityList) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        var savedData = await _context.Productivities.ToListAsync();
        Assert.Empty(savedData);
    }

    [Fact]
    public async Task Edit_IdIsNull_ShouldReturnNotFound()
    {
        var result = await _controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ProductivityNotFound_ShouldReturnNotFound()
    {
        var result = await _controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

 



}