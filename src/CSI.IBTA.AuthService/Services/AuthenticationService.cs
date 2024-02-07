using CSI.IBTA.AuthService.DTOs;
using CSI.IBTA.AuthService.Interfaces;

namespace CSI.IBTA.AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<LoginResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
