using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class UserManagementContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }

    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                RoleName = "Employee"
            },
            new Role
            {
                Id = 2,
                RoleName = "EmployerAdmin"
            },
            new Role
            {
                Id = 3,
                RoleName = "Administrator"
            }
        );

        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1,
                Username = "Admin",
                Password = "1000:+cW1d+J1cdzepfIFXH2lTzJUXFhFt6wO:2YU157R4anp5Y9Qrdbh5tRXo2KIhV8Ik",
                RoleId = 3
            }
        );
    }
}
