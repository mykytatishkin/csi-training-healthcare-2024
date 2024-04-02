using CSI.IBTA.AuthService.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Constants;

namespace CSI.IBTA.AuthService.Authentication
{
    internal class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(int accountId, int? employerId, int? userId, string role)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new List<System.Security.Claims.Claim>
            {
                new(JwtRegisteredClaimNames.Sub, accountId.ToString()),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (employerId != null) claims.Add(new(JwtTokenClaimConstants.EmployerId, ((int) employerId).ToString()));
            else if (userId != null) claims.Add(new(JwtTokenClaimConstants.UserId, ((int) userId).ToString()));
            
            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
