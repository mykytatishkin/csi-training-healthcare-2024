using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUsersService
    {
        public Task<GenericHttpResponse<IEnumerable<UserDto>>> GetAllUsers();
        public Task<GenericHttpResponse<UserDto>> GetUserByAccountId(int accountId);
        public Task<GenericHttpResponse<UserDto>> GetUser(int userId);
        public Task<GenericHttpResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto);
        public Task<GenericHttpResponse<UpdatedUserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto);
        public Task<GenericHttpResponse<UpdatedUserDto>> PutUser(int userId, PutUserDto putUserDto);
        public Task<GenericHttpResponse<bool>> DeleteUser(int userId);
    }
}