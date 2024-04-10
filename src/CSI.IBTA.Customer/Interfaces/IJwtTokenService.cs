namespace CSI.IBTA.Customer.Interfaces
{
    public interface IJwtTokenService
    {
        bool IsCustomer(string token);
        CookieOptions GetCookieOptions();
        bool IsTokenValid(string token);
        string? GetCachedToken();
    }
}
