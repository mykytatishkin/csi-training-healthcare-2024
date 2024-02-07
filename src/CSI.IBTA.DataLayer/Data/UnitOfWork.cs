using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.DataLayer.Repositories;
using CSI.IBTA.Shared.IConfiguration;
using CSI.IBTA.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CsiHealthcare2024Context _context;
        private readonly ILogger _logger;

        public IUserRepository Users { get; private set; }
        public IAccountRepository Accounts { get; private set; }
        public IEmployerRepository Employers { get; private set; }

        public UnitOfWork(CsiHealthcare2024Context context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Users = new UserRepository(context, _logger);
            Accounts = new AccountRepository(context, _logger);
            Employers = new EmployerRepository(context, _logger);
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
