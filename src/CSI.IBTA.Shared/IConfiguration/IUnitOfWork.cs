using CSI.IBTA.Shared.Interfaces;

namespace CSI.IBTA.Shared.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        IEmployerRepository Employers { get; }

        Task CompleteAsync();
    }
}
