using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUsersService
    {
        public Task<GenericResponse<IEnumerable<UserDto>>> GetAllUsers();
        public Task<GenericResponse<UserDto>> GetUserByAccountId(int accountId);
        public Task<GenericResponse<UserDto>> GetUser(int userId);
        public Task<GenericResponse<IEnumerable<UserDto>>> GetUsers(List<int> userIds);
        public Task<GenericResponse<IEnumerable<UserDto>>> GetUsersByUsernames(List<string> usernames);
        public Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto);
        public Task<GenericResponse<UpdatedUserDto>> PutUser(int userId, PutUserDto putUserDto);
        public Task<GenericResponse<bool>> DeleteUser(int userId);
    }
}