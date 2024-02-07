using CSI.IBTA.DataLayer.Data;
using CSI.IBTA.Shared.IConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace CSI.IBTA.DataLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
