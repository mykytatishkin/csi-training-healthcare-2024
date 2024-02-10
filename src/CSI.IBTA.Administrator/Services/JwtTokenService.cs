using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using CSI.IBTA.Administrator.Interfaces;

namespace CSI.IBTA.Administrator.Services
{
    public class JwtTokenService : IJwtTokenService
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

        public async Task<(bool isAdmin, string token)> CheckUserIsAdminAsync(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenObject = JsonConvert.DeserializeObject<JToken>(responseContent);
            var token = tokenObject?["token"]?.Value<string>();

            if (token == null)
            {
                _logger.LogError("Failed to deserialize token");
                return (false, "");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var role = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?
                .Value;

            return role == "Administrator" ? (true, token) : (false, "");
        }
    }
}
