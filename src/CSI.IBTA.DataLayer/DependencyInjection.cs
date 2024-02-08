using CSI.IBTA.DataLayer.Data;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSI.IBTA.DataLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<UserManagementContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly("CSI.IBTA.DB.Migrations")));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();            
            
            return services;
        }
    }
}
