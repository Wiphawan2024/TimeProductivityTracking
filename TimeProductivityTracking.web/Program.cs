using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Data;
using DinkToPdf; //for pdf
using DinkToPdf.Contracts;
using System.Runtime.InteropServices;//for pdf

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("IdentityAuthContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityAuthContextConnection' not found.");

builder.Services.AddDbContext<IdentityAuthContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityAuthUser>
    (options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add role 
    .AddEntityFrameworkStores<IdentityAuthContext>();

// Manually load the wkhtmltox DLL
// Set the correct DLL path
var wkhtmltoxPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "wkhtmltopdf", "wkhtmltox.dll");
if (File.Exists(wkhtmltoxPath))
{
    NativeLibrary.Load(wkhtmltoxPath);
}
else
{
    throw new Exception($"wkhtmltopdf DLL not found at {wkhtmltoxPath}");
}

// Register DinkToPdf service
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));



//1. Add connect database

builder.Services.AddDbContext<ProductivitiesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductivitiesContext") ?? throw new InvalidOperationException("Connection string 'ProductivitiesContext' not found.")));


builder.Services.AddControllersWithViews();

var app = builder.Build();
//Initialize Data
using (var scope = app.Services.CreateScope())
{
    var service=scope.ServiceProvider;
    DbInitializer.Initializer(service); //Initialize SEC Contracts

    await SeedDBInitialize.InitializeAsync(service);
}

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();//Identity
app.Run();
