using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class UserManagementContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
    {
    }
}
