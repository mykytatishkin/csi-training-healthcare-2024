using CSI.IBTA.Shared.Constants;
using System.Security.Claims;

namespace CSI.IBTA.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetEmployerId(this ClaimsPrincipal user)
        {
            var employerIdClaim = user.Claims.FirstOrDefault(c => c.Type == JwtTokenClaimConstants.EmployerId);

            if (employerIdClaim == null || !int.TryParse(employerIdClaim.Value, out int employerId))
                return null;

            return employerId;
        }
    }
}
