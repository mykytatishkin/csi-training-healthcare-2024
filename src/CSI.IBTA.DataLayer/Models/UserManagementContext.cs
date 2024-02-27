using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class UserManagementContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<EmployerUser> EmployerUsers { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanType> PlanTypes { get; set; }

    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1,
                Username = "Admin",
                // Password: admin
                Password = "1000:+cW1d+J1cdzepfIFXH2lTzJUXFhFt6wO:2YU157R4anp5Y9Qrdbh5tRXo2KIhV8Ik",
                Role = Role.Administrator
            }
        );
        modelBuilder.Entity<Account>()
        .HasOne(e => e.User)
        .WithOne(e => e.Account)
        .HasForeignKey<User>("AccountId");

        modelBuilder.Entity<Employer>()
            .HasMany(x => x.Settings)
            .WithOne(x => x.Employer)
            .IsRequired();
   
    }
}
