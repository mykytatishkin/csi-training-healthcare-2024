using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class BenefitsManagementContext : DbContext
{
    public DbSet<Claim> Claim { get; set; }
    public DbSet<Enrollment> Enrollment { get; set; }
    public DbSet<Package> Package { get; set; }
    public DbSet<Plan> Plan { get; set; }
    public DbSet<PlanType> PlanType { get; set; }

    public BenefitsManagementContext(DbContextOptions<BenefitsManagementContext> options)
        : base(options)
    {

    }

}
