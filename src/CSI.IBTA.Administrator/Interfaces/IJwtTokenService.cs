namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IJwtTokenService
    {
        bool IsAdmin(string token);
        CookieOptions GetCookieOptions();
        bool IsTokenValid(string token);
    }
}
