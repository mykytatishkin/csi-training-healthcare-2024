using CSI.IBTA.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace CSI.IBTA.Customer.Extensions
{
    public static class JwtSecurityTokenExtensions
    {
        public static int? GetEmployeeId(this string? token)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var employeeIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == JwtTokenClaimConstants.UserId);

            if (employeeIdClaim == null || !int.TryParse(employeeIdClaim.Value, out int employeeId))
            {
                return null;
            }

            return employeeId;
        }
    }
}
