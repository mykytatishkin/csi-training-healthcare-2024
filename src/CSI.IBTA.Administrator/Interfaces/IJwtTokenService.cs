using Newtonsoft.Json.Linq;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IJwtTokenService
    {
        (bool isAdmin, string token) IsAdmin(JToken token);
        CookieOptions GetCookieOptions();
    }
}
