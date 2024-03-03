using CSI.IBTA.Administrator.Clients;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Services;
using CSI.IBTA.AuthService.Authentication;
using Microsoft.Extensions.Options;

namespace CSI.IBTA.Administrator
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdministratorPortal(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));

            services.AddLogging();
            services.AddHttpContextAccessor();
            services.AddTransient<AuthorizedHttpClient>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserServiceClient, UserServiceClient>();
            services.AddScoped<IEmployerUserClient, EmployerUserClient>();
            services.AddScoped<IEmployerClient, EmployerClient>();
            services.AddScoped<IInsurancePackageClient, InsurancePackageClient>();
            services.AddScoped<IBenefitsClient, BenefitsClient>();
            services.AddHttpClient<IAuthClient, AuthClient>();
            return services;
        }
    }
}
