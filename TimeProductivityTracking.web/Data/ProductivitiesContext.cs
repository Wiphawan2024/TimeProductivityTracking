using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using OfficeManagement.Models;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Data
{
    public class ProductivitiesContext : DbContext
    {
        public ProductivitiesContext(DbContextOptions<ProductivitiesContext> options) : base(options) { }
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<SECContract> SECContracts { get; set; }
        public DbSet<Productivities> ProductivitieS { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().ToTable("UserInfo");
            modelBuilder.Entity<SECContract>().ToTable("SECContract");
            modelBuilder.Entity<Productivities>().ToTable("Productivities");
            modelBuilder.Entity<Rate>().ToTable("Rate");
            modelBuilder.Entity<Contractor>().ToTable("Contractor");
 
              




        }
    }
}
