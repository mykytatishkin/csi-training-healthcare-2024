using CSI.IBTA.Shared;
namespace CSI.IBTA.UserService.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> GetUser(AccountDto account);
    }
}
