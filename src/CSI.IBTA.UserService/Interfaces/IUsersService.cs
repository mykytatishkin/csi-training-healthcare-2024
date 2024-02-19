using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUsersService
    {
        
        public Task<GenericResponse<UserDto[]>> GetAllUsers(HttpContext httpContext);
        public Task<GenericResponse<UserDto>> GetUser(int accountId, HttpContext httpContext);
        public Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto, HttpContext httpContext);
        public Task<GenericResponse<UpdatedUserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto, HttpContext httpContext);
        public Task<GenericResponse<bool>> DeleteUser(int userId, HttpContext httpContext);
    }
}
