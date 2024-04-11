using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Interfaces
{
    public interface IEmployeesClient
    {
        Task<GenericResponse<FullEmployeeDto>> GetEmployee(int id);
        Task<GenericResponse<string?>> GetEmployerLogo();
    }
}