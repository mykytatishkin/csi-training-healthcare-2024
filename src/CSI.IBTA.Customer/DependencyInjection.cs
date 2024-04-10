using CSI.IBTA.Customer.Clients;
using CSI.IBTA.Customer.Interfaces;

namespace CSI.IBTA.Customer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomerPortal(this IServiceCollection services)
        {
            services.AddScoped<IEmployeesClient, EmployeesClient>();
            services.AddScoped<IEnrollmentsClient, EnrollmentsClient>();
            return services;
        }
    }
}
