using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.DataLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Account> Accounts { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Employer> Employers { get; }
        IGenericRepository<Address> Addresses { get; }
        IGenericRepository<Email> Emails { get; }
        IGenericRepository<Phone> Phones { get; }
        IGenericRepository<EmployerUser> EmployerUsers { get; }
        IGenericRepository<Settings> Settings { get; }
        IGenericRepository<Claim> Claims { get; }
        IGenericRepository<Enrollment> Enrollments { get; }
        IGenericRepository<Package> Packages { get; }
        IGenericRepository<Plan> Plans { get; }
        IGenericRepository<PlanType> PlanTypes { get; }

        Task CompleteAsync();
    }
}
