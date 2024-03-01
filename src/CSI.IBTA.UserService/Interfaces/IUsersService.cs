using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUsersService
    {
        public Task<GenericResponse<UserDto[]>> GetAllUsers();
        public Task<GenericResponse<UserDto>> GetUserByAccountId(int accountId);
        public Task<GenericResponse<UserDto>> GetUser(int userId);
        public Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto);
        public Task<GenericResponse<UpdatedUserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto);
        public Task<GenericResponse<bool>> DeleteUser(int userId);
    }
}
