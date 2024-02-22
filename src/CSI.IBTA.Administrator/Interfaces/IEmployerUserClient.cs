using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IEmployerUserClient
    {
        Task<GenericInternalResponse<List<UserDto>>> GetEmployerUsers(int employerId);
        Task<GenericInternalResponse<bool?>> CreateEmployerUser(CreateUserDto command);
        Task<GenericInternalResponse<bool?>> UpdateEmployerUser(PutUserDto command, int accountId);
    }
}
