namespace CSI.IBTA.Employer.Interfaces
{
    public interface IJwtTokenService
    {
        bool IsAdmin(string token);
        CookieOptions GetCookieOptions();
        bool IsTokenValid(string token);
        string? GetCachedToken();
        int? GetEmployerId(string token);
    }
}
