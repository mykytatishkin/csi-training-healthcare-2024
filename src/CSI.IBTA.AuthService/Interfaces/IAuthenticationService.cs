using CSI.IBTA.AuthService.DTOs;

namespace CSI.IBTA.AuthService.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<LoginResponse> Login(LoginRequest request);
    }
}
