using CSI.IBTA.Administrator.Clients;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Services;
using CSI.IBTA.AuthService.Authentication;
using Microsoft.Extensions.Options;
using System.Reflection;

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
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddHttpClient<IAuthClient, AuthClient>();
            services.AddHttpClient<IEmployerUserClient, EmployerUserClient>();
            services.AddHttpClient<IUserServiceClient, UserServiceClient>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
