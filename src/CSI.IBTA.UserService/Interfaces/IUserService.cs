using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUsersService
    {
        
        public Task<UserDto?> GetUser(int accountId, HttpContext httpContext);
        public Task<NewUserDto?> CreateUser(CreateUserDto createUserDto, HttpContext httpContext);
        public Task<NewUserDto?> UpdateUser(int userId, UpdateUserDto updateUserDto, HttpContext httpContext);
        public Task<bool> DeleteUser(int userId, HttpContext httpContext);
    }
}
