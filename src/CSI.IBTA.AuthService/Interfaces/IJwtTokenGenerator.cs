namespace CSI.IBTA.AuthService.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int accountId, int? employerId, int? userId, string role);
    }
}