using CSI.IBTA.Shared;

namespace CSI.IBTA.AuthService.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<LoginResponse> Login(LoginRequest request);
    }
}
