using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<APIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure())); // Enables retry logic for transient errors

// Configure Database & Identity
builder.Services.AddDbContext<APIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TimeProductivityTracking.API"))); // Ensure EF migrations stay in API

builder.Services.AddIdentity<IdentityAuthUser, IdentityRole>()
    .AddEntityFrameworkStores<APIContext>()
    .AddDefaultTokenProviders();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //
app.UseAuthorization();

app.UseCors("AllowAll"); // 
app.MapControllers();

app.Run();
