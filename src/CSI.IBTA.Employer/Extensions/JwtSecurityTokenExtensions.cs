using CSI.IBTA.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace CSI.IBTA.Employer.Extensions
{
    public static class JwtSecurityTokenExtensions
    {
        public static int? GetEmployerId(this string? token)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var employerIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == JwtTokenClaimConstants.EmployerId);

            if (employerIdClaim == null || !int.TryParse(employerIdClaim.Value, out int employerId))
            {
                return null;
            }

            return employerId;
        }
    }
}
