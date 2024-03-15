using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class BenefitsManagementContext : DbContext
{
    public DbSet<Claim> Claim { get; set; }
    public DbSet<Enrollment> Enrollment { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Package> Package { get; set; }
    public DbSet<Plan> Plan { get; set; }
    public DbSet<PlanType> PlanType { get; set; }

    public BenefitsManagementContext(DbContextOptions<BenefitsManagementContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>()
            .HasOne(e => e.PlanType)
            .WithMany()
            .HasForeignKey(e => e.TypeId);
        modelBuilder.Entity<PlanType>().HasData(
            new PlanType
            {
                Id = 1,
                Name = "Medical",
            },
            new PlanType
            {
                Id = 2,
                Name = "Dental",
            }
        );

        modelBuilder.Entity<Package>()
            .HasMany(x => x.Plans)
            .WithOne(x => x.Package)
            .IsRequired();
    }
}