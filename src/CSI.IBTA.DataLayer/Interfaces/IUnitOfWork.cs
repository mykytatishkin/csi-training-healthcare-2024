using CSI.IBTA.DataLayer.Repositories;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.DataLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Account> Accounts { get; }

        Task CompleteAsync();
    }
}
