using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.DataLayer.Interfaces
{
    public interface IBenefitsUnitOfWork
    {
        IGenericRepository<Claim> Claims { get; }
        IGenericRepository<Enrollment> Enrollments { get; }
        IGenericRepository<Package> Packages { get; }
        IGenericRepository<Plan> Plans { get; }
        IGenericRepository<PlanType> PlanTypes { get; }
        IGenericRepository<Transaction> Transactions { get; }
        Task CompleteAsync();
    }
}
