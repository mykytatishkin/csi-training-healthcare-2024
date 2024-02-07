using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.AuthService.Services;
using CSI.IBTA.DataLayer.Models;

namespace CSI.IBTA.AuthService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthService(this IServiceCollection services)
        {
            services.AddDbContext<CsiHealthcare2024Context>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
