using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.DataLayer.Repositories;
using CSI.IBTA.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace CSI.IBTA.DataLayer.Data
{
    public class BenefitsUnitOfWork : IBenefitsUnitOfWork, IDisposable
    {
        private readonly BenefitsManagementContext _context;
        private readonly ILogger _logger;

        public IGenericRepository<Claim> Claims { get; private set; }
        public IGenericRepository<Enrollment> Enrollments { get; private set; }
        public IGenericRepository<Package> Packages { get; private set; }
        public IGenericRepository<Transaction> Transactions { get; private set; }
        public IGenericRepository<Plan> Plans { get; private set; }
        public IGenericRepository<PlanType> PlanTypes { get; private set; }

        public BenefitsUnitOfWork(BenefitsManagementContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Claims = new GenericRepository<Claim>(context, _logger);
            Enrollments = new GenericRepository<Enrollment>(context, _logger);
            Packages = new GenericRepository<Package>(context, _logger);
            Plans = new GenericRepository<Plan>(context, _logger);
            PlanTypes = new GenericRepository<PlanType>(context, _logger);
            Transactions = new GenericRepository<Transaction>(context, _logger);
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
