using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IAuthClient
    {
        Task<GenericResponse<bool>> Authenticate(LoginRequest request);
    }
}
