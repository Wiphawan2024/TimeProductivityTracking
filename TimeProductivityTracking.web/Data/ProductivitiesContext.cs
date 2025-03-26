using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Data
{
    public class ProductivitiesContext :  DbContext// IdentityDbContext<IdentityAuthUser> // Use IdentityDbContext
    {
        public ProductivitiesContext(DbContextOptions<ProductivitiesContext> options) : base(options) { }

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<SECContract> SECContracts { get; set; }
        public DbSet<Productivity> Productivities { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensures Identity configurations are applied

            modelBuilder.Entity<UserInfo>().ToTable("UserInfo");
            modelBuilder.Entity<SECContract>().ToTable("SECContract");
            modelBuilder.Entity<Productivity>().ToTable("Productivities");
            modelBuilder.Entity<Productivity>().Property(p => p.AchevedDays)
                .HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Productivity>().Property(p=>p.PlannedDays)
                .HasColumnType("decimal(5, 2)");


           modelBuilder.Entity<Rate>().ToTable("Rate");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<Productivity>()
    .HasOne(p => p.Contractor)
    .WithMany()
    .HasForeignKey(p => p.ContractorId)
    .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
