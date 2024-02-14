using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.AuthService.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CSI.IBTA.Administrator.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        private readonly ILogger<JwtTokenService> _logger;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(ILogger<JwtTokenService> logger, IOptions<JwtSettings> jwtSettings)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        public CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true
            };
        }

        public bool IsAdmin(string token)
        {
            if (token == null)
            {
                _logger.LogError("Failed to get token value");
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var role = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?
                .Value;

            return role == Role.Administrator.ToString();
        }

        public bool IsTokenValid(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
