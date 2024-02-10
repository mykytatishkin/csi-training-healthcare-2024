
namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IJwtTokenService
    {
        Task<(bool isAdmin, string token)> CheckUserIsAdminAsync(HttpResponseMessage response);
        CookieOptions GetCookieOptions();
    }
}
