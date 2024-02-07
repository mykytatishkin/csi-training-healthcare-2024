using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.AuthService.DTOs
{
    public record LoginResponse(User User, string Token);
}
