using CSI.IBTA.AuthService.DTOs;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.AuthService.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<GenericResponse<LoginResponse>> Login(LoginRequest request);
    }
}
