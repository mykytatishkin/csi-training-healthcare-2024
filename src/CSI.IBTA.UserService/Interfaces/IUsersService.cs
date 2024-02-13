using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUsersService
    {
        
        public Task<UserDto?> GetUser(int accountId);
        public Task<NewUserDto?> CreateUser(CreateUserDto createUserDto);
        public Task<NewUserDto?> UpdateUser(int userId, UpdateUserDto updateUserDto);
        public Task<bool> DeleteUser(int userId);
    }
}
