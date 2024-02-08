using CSI.IBTA.AuthService.Authentication;
using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.AuthService.Services;
using Microsoft.Extensions.Options;

namespace CSI.IBTA.AuthService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthService(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
