using CSI.IBTA.Shared.DTOs.Login;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserServiceClient
    {
        Task<IQueryable<Employer>?> GetEmployers(string token);
    }
}
