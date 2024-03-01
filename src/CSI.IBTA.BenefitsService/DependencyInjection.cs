using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.BenefitsService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CSI.IBTA.BenefitsService
{
    public static class DependencyInjection {
        public static IServiceCollection AddBenefitsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IInsurancePackageService, InsurancePackageService>();
            services.AddAuth(configuration);

            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidAudience = configuration["JwtSettings:Audience"];
                options.TokenValidationParameters.ValidIssuer = configuration["JwtSettings:Issuer"];
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
            });

            services.AddAuthorization();
            return services;
        }
    }
}