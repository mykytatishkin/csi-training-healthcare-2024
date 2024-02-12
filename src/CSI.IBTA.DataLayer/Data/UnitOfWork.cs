using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.DataLayer.Repositories;
using CSI.IBTA.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserManagementContext _context;
        private readonly ILogger _logger;

        public IGenericRepository<Account> Accounts { get; private set; }
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Role> Roles { get; private set; }

        public UnitOfWork(UserManagementContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Accounts = new GenericRepository<Account>(context, _logger);
            Users = new GenericRepository<User>(context, _logger);
            Roles = new GenericRepository<Role>(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
