using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.DataLayer.Repositories;
using CSI.IBTA.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CsiHealthcare2024Context _context;
        private readonly ILogger _logger;

        public IGenericRepository<User> Accounts { get; private set; }

        public UnitOfWork(CsiHealthcare2024Context context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Accounts = new GenericRepository<User>(context, _logger);
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
