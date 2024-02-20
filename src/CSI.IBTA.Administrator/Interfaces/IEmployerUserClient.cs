using CSI.IBTA.Administrator.DTOs.EmployerUser;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IEmployerUserClient
    {
        Task<GenericInternalResponse<bool?>> CreateEmployerUser(CreateUserDto command, string token);
    }
}
