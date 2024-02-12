using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUserService
    {
        
        public Task<UserDto?> GetUser(int accountId);
        public Task<NewUserDto?> CreateUser(CreateUserDto createUserDto);
        public Task<NewUserDto?> UpdateUser(int userId, UpdateUserDto updateUserDto);
        public Task<bool> DeleteUser(int userId);
    }
}
