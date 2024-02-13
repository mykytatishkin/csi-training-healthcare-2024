using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared;

namespace CSI.IBTA.AuthService.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<GenericResponse<LoginResponse>> Login(LoginRequest request);
    }
}
