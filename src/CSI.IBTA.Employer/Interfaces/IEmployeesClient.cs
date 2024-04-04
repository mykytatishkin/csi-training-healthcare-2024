using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IEmployeesClient
    {
        Task<GenericResponse<PagedEmployeesResponse>> GetEmployees(
            int page,
            int pageSize,
            int employerId,
            string firstname = "",
            string lastname = "",
            string ssn = "");

        Task<GenericResponse<bool?>> CreateEmployee(CreateEmployeeDto command);

        Task<GenericResponse<CreateEmployeeDto>> GetEmployee(int id);

        Task<GenericResponse<bool?>> UpdateEmployee(int? id, CreateEmployeeDto command);
    }
}