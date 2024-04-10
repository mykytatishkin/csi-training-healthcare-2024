﻿using CSI.IBTA.Customer.Clients;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Services;
using CSI.IBTA.Customer.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CSI.IBTA.Customer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomerPortal(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddHttpContextAccessor();
            services.AddTransient<AuthorizedHttpClient>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmployeesClient, EmployeesClient>();
            services.AddScoped<IEnrollmentsClient, EnrollmentsClient>();
            services.AddHttpClient<IAuthClient, AuthClient>();
            return services;
        }
    }
}
