using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        private readonly ILogger<JwtTokenService> _logger;
        public JwtTokenService(ILogger<JwtTokenService> logger)
        {
            _logger = logger;
        }

        public CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true
            };
        }

        public (bool isAdmin, string token) IsAdmin(JToken token)
        {
            var t = token["token"]?.Value<string>();

            if (t == null)
            {
                _logger.LogError("Failed to get token value");
                return (false, "");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(t);
            var role = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?
                .Value;

            return role == Role.Administrator.ToString() ? (true, t) : (false, "");
        }
    }
}
