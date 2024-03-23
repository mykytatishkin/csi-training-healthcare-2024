using Microsoft.AspNetCore.Authentication.JwtBearer;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.IdentityModel.Tokens;
using CSI.IBTA.UserService.Services;
using System.Reflection;
using System.Text;

namespace CSI.IBTA.UserService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IEmployersService, EmployersService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
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
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
            });

            services.AddAuthorization();
            return services;
        }
    }
}