namespace CSI.IBTA.Customer.Interfaces
{
    public interface IJwtTokenService
    {
        bool IsAdmin(string token);
        CookieOptions GetCookieOptions();
        bool IsTokenValid(string token);
        string? GetCachedToken();
    }
}
