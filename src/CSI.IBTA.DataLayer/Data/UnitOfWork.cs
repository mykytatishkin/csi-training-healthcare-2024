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
        public IGenericRepository<EmployerUser> EmployerUsers { get; private set; }
        public IGenericRepository<Settings> Settings { get; private set; }
        public IGenericRepository<Claim> Claims { get; private set; }
        public IGenericRepository<Enrollment> Enrollments { get; private set; }
        public IGenericRepository<Package> Packages { get; private set; }
        public IGenericRepository<Plan> Plans { get; private set; }
        public IGenericRepository<PlanType> PlanTypes { get; private set; }

        public UnitOfWork(UserManagementContext userContext, BenefitsManagementContext benefitsContext, ILoggerFactory loggerFactory)
        {
            _context = userContext;
            _logger = loggerFactory.CreateLogger("logs");

            Accounts = new GenericRepository<Account>(userContext, _logger);
            Users = new GenericRepository<User>(userContext, _logger);
            Employers = new GenericRepository<Employer>(userContext, _logger);
            Emails = new GenericRepository<Email>(userContext, _logger);
            Phones = new GenericRepository<Phone>(userContext, _logger);
            Addresses = new GenericRepository<Address>(userContext, _logger);
            EmployerUsers = new GenericRepository<EmployerUser>(userContext, _logger);
            Settings = new GenericRepository<Settings>(userContext, _logger);
            Claims = new GenericRepository<Claim>(benefitsContext, _logger);
            Enrollments = new GenericRepository<Enrollment>(benefitsContext, _logger);
            Packages = new GenericRepository<Package>(benefitsContext, _logger);
            Plans = new GenericRepository<Plan>(benefitsContext, _logger);
            PlanTypes = new GenericRepository<PlanType>(benefitsContext, _logger);
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
