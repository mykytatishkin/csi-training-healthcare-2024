using Microsoft.AspNetCore.Authentication.JwtBearer;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.IdentityModel.Tokens;
using CSI.IBTA.UserService.Services;
using System.Reflection;
using System.Text;
using CSI.IBTA.UserService.Authorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using CSI.IBTA.UserService.Authorization.Policies.Handlers;
using CSI.IBTA.UserService.Authorization.Constants;

namespace CSI.IBTA.UserService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IEmployersService, EmployersService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IEncodingService, EncodingService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!));
            });

            services.AddSingleton<IAuthorizationHandler, EmployerAdminOwnerRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, EmployeeOwnerRequirementHandler>();
            services.AddAuthorization(o =>
            {
                o.AddPolicy(PolicyConstants.EmployerAdminOwner, policy =>
                    policy.Requirements.Add(new EmployerAdminOwnerRequirement()));
                o.AddPolicy(PolicyConstants.EmployeeOwner, policy =>
                    policy.Requirements.Add(new EmployeeOwnerRequirement()));
            });
            return services;
        }
    }
}