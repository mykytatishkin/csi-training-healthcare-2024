using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IEmployerUserClient
    {
        Task<GenericResponse<List<UserDto>>> GetEmployerUsers(int employerId);
        Task<GenericResponse<bool?>> CreateEmployerUser(CreateUserDto command);
        Task<GenericResponse<bool?>> UpdateEmployerUser(PutUserDto command, int accountId);
    }
}
