using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Controllers;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;
using Xunit;

public class InvoiceCalculationTests
{
    [Fact]
    public async Task Approve_ShouldCalculateCorrectInvoiceAmount()
    {
        // Arrange: create in-memory db context
        var options = new DbContextOptionsBuilder<ProductivitiesContext>()
            .UseInMemoryDatabase(databaseName: "InvoiceTestDb")
            .Options;
        await using var context=new ProductivitiesContext(options);

        // Add test Rate
        var rate=new Rate
        {
            RateID = 1,
            RateName = "L5",
            HourlyWage = 65.0
        };

        context.Rates.Add(rate);

        // Add test UserInfo
        var contractor=new UserInfo
        {
            UserId = 1,
            FName = "Test",
            LName = "User",
            Email = "contractor@sec.ie",
            RateID = 1,
            Rate=rate
        };

       context.Users.Add(contractor);


        // Seed productivity
        context.Productivities.AddRange(new List<Productivity>
        {
            new Productivity { ContractorId = 1, Monthly = "May 2025", AchevedDays = 5 },
            new Productivity { ContractorId = 1, Monthly = "May 2025", AchevedDays = 3 },
        });

        await context.SaveChangesAsync();

        var controller = new ProductivitySummaryViewModelsController(context, null!);

        // Act
        var result = await controller.Approve("May 2025", contractor.UserId);

        // Assert: check that invoice was created
        var invoice = await context.Invoices.FirstOrDefaultAsync();
        Assert.NotNull(invoice);
        Assert.Equal(8, invoice.TotalHours); // 5 + 3
        Assert.Equal(520, invoice.TotalAmount); // 8 hrs * €20
    }
}
