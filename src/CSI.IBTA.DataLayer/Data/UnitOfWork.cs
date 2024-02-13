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
        public IGenericRepository<Email> Emails{ get; private set; }
        public IGenericRepository<Phone> Phones { get; private set; }
        public IGenericRepository<Address> Addresses { get; private set; }
        public IGenericRepository<Employer> Employers { get; private set; }

        public UnitOfWork(UserManagementContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Accounts = new GenericRepository<Account>(context, _logger);
            Users = new GenericRepository<User>(context, _logger);
            Employers = new GenericRepository<Employer>(context, _logger);
            Emails = new GenericRepository<Email>(context, _logger);
            Phones = new GenericRepository<Phone>(context, _logger);
            Addresses = new GenericRepository<Address>(context, _logger);
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
