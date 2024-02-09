using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class UserManagementContext : DbContext
{
    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
    {
    }
}
