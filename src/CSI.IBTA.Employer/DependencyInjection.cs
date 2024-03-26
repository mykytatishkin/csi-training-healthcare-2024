using CSI.IBTA.Employer.Clients;
using CSI.IBTA.Employer.Interfaces;

namespace CSI.IBTA.Employer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployerPortal(this IServiceCollection services)
        {
            services.AddTransient<AuthorizedHttpClient>();
            services.AddScoped<IEmployeesClient, EmployeesClient>();
            services.AddScoped<IEmployersClient, EmployersClient>();
            return services;
        }
    }
}