using System.IdentityModel.Tokens.Jwt;
using CSI.IBTA.Consumer.Interfaces;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Consumer.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CSI.IBTA.Consumer.Constants;

namespace CSI.IBTA.Consumer.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JwtTokenService> _logger;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(
            ILogger<JwtTokenService> logger, 
            IOptions<JwtSettings> jwtSettings, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
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

            return role == Role.Employee.ToString();
        }

        public bool IsTokenValid(string token)
        {
            if (_jwtSettings.Secret == null)
            {
                _logger.LogError("JWT secret key is not configured.");
                return false;
            }

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


        public string? GetCachedToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                return null;
            }

            string? token = httpContext.Request.Cookies[TokenConstants.JwtTokenCookieName];

            if (token == null)
            {
                return null;
            }

            return token;
        }
    }
}
