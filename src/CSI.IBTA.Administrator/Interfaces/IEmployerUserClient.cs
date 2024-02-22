using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IEmployerUserClient
    {
        Task<GenericInternalResponse<bool?>> CreateEmployerUser(CreateUserDto command, string token);
        Task<GenericInternalResponse<bool?>> UpdateEmployerUser(PutUserDto command, int accountId, string token);
    }
}
