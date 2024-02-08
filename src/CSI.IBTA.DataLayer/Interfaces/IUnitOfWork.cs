using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.DataLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Account> Accounts { get; }
        IGenericRepository<Role> Roles { get; }

        Task CompleteAsync();
    }
}
