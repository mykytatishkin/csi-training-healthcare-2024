using CSI.IBTA.Shared;
using CSI.IBTA.Shared.Types;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IAuthClient
    {
        Task<AuthenticationResult> Authenticate(LoginRequest request);
    }
}
