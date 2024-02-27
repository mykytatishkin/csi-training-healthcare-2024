using CSI.IBTA.DataLayer.Data;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSI.IBTA.DataLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserUnitOfWork(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<UserManagementContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly("CSI.IBTA.DB.Migrations")));

            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();            
            
            return services;
        }

        public static IServiceCollection AddBenefitsUnitOfWork(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<BenefitsManagementContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly("CSI.IBTA.DB.Migrations")));

            services.AddScoped<IBenefitsUnitOfWork, BenefitsUnitOfWork>();

            return services;
        }
    }
}
