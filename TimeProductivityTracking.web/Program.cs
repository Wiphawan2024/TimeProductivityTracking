using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Data;


var builder = WebApplication.CreateBuilder(args);

//  Read Connection Strings
//var identityConnection = builder.Configuration.GetConnectionString("IdentityAuthContextConnection");
//
//var productivityConnection = builder.Configuration.GetConnectionString("ProductivitiesContext");



var connectionString = builder.Configuration.GetConnectionString("IdentityAuthContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityAuthContextConnection' not found.");

builder.Services.AddDbContext<IdentityAuthContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityAuthUser>
    (options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add role 
    .AddEntityFrameworkStores<IdentityAuthContext>();

builder.Services.AddDbContext<ProductivitiesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductivitiesContext") ??
    throw new InvalidOperationException("Connection string 'ProductivitiesContext' not found.")));



//  MVC Setup
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    try
    {
        Console.WriteLine("✔ Seeding database...");
        DbInitializer.Initializer(service);
        await SeedDBInitialize.InitializeAsync(service);
        Console.WriteLine("✔ Database seeding completed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error during DB seeding: " + ex.Message);
        throw;
    }
}

//  Middleware Pipeline
try
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication(); // Needed for Identity
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages(); // Enables Identity UI
    app.Run();
    Console.WriteLine("✔ Application started successfully.");
}
catch (Exception ex)
{
    Console.WriteLine("❌ Error during app initialization: " + ex.Message);
    throw;
}
