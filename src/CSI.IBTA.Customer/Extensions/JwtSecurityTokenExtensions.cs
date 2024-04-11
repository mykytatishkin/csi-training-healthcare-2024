using CSI.IBTA.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace CSI.IBTA.Customer.Extensions
{
    public static class JwtSecurityTokenExtensions
    {
        public static int? GetUserId(this string? token)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var employerIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == JwtTokenClaimConstants.UserId);

            if (employerIdClaim == null || !int.TryParse(employerIdClaim.Value, out int employerId))
            {
                return null;
            }

            return employerId;
        }
    }
}
