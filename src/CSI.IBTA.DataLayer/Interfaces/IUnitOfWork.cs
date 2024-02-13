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

        Task CompleteAsync();
    }
}
