using Microsoft.AspNetCore.Identity;
using TimeProductivityTracking.web.Areas.Identity.Data;

namespace TimeProductivityTracking.web.Areas.Identity.Data
{
    public class SeedDBInitialize
    {
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
        }

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
    }
}
