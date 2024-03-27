using CSI.IBTA.Employer.Clients;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Services;
using CSI.IBTA.Employer.Authentication;
using Microsoft.Extensions.Options;

namespace CSI.IBTA.Employer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployerPortal(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));

            services.AddTransient<AuthorizedHttpClient>();
            services.AddScoped<IEmployeesClient, EmployeesClient>();
            services.AddScoped<IEmployersClient, EmployersClient>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddHttpClient<IAuthClient, AuthClient>();
            return services;
        }
    }
}
