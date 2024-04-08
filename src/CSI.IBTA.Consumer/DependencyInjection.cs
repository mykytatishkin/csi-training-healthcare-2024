using CSI.IBTA.Consumer.Clients;
using CSI.IBTA.Consumer.Interfaces;
using CSI.IBTA.Consumer.Services;
using CSI.IBTA.Consumer.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CSI.IBTA.Consumer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConsumerPortal(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddHttpContextAccessor();
            services.AddTransient<AuthorizedHttpClient>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddHttpClient<IAuthClient, AuthClient>();
            return services;
        }
    }
}
