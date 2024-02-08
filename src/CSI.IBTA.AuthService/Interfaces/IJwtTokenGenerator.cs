namespace CSI.IBTA.AuthService.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int accountId, string role);
    }
}