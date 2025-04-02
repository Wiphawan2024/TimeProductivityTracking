using Microsoft.AspNetCore.Identity;
using TimeProductivityTracking.web.Areas.Identity.Data;
using System.Threading.Tasks;

namespace TimeProductivityTracking.web.Areas.Identity.Data
{
    public class SeedDBInitialize
    {
        // Seed roles and admin user if they don't exist
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (_roleManager != null)
            {
                await CreateRoleIfNotExists(_roleManager, "Admin");
                await CreateRoleIfNotExists(_roleManager, "Manager");
                await CreateRoleIfNotExists(_roleManager, "Contractor"); // Ensure Contractor is added
                await CreateRoleIfNotExists(_roleManager, "HR");
            }

            var _userManager = serviceProvider.GetRequiredService<UserManager<IdentityAuthUser>>();
            if (_userManager != null)
            {
                await CreateAdminUserIfNotExists(_userManager);
            }
        }

        // Ensure roles exist
        private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Error creating role {roleName}: {string.Join(",", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        // Create Admin user if not exists
        private static async Task CreateAdminUserIfNotExists(UserManager<IdentityAuthUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@domain.com");

            if (adminUser == null)
            {
                adminUser = new IdentityAuthUser
                {
                    UserName = "admin@domain.com",
                    Email = "admin@domain.com",
                   EmailConfirmed = true // Setting EmailConfirmed to true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!"); // Make sure to use a secure password

                if (result.Succeeded)
                {
                    // Assign the "Admin" role to the user
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Admin user created successfully.");
                }
                else
                {
                    Console.WriteLine($"Error creating Admin user: {string.Join(",", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
