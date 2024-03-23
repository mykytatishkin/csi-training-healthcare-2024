using CSI.IBTA.Employer.Clients;
using CSI.IBTA.Employer.Interfaces;

namespace CSI.IBTA.Employer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployerPortal(this IServiceCollection services)
        {
            services.AddTransient<AuthorizedHttpClient>();
            services.AddScoped<IEmployeeClient, EmployeesClient>();
            return services;
        }
    }
}
