
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.API.Models;
namespace TimeProductivityTracking.API.Data
{
    public class APIContext : IdentityDbContext<IdentityAuthUser>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        public DbSet<UserInfo> Users { get; set; }
    }
}
