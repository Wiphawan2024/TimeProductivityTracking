using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Data
{
    public class ProductivitiesContext:DbContext
    {
        public ProductivitiesContext(DbContextOptions<ProductivitiesContext> options) : base(options) { }

        public DbSet<SECContract> SECContracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SECContract>().ToTable("SECContract");
          
        }
    }
}
