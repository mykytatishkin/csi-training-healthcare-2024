using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployeesService
    {
        Task<GenericResponse<PagedEmployeesResponse>> GetEmployees(int page, int pageSize, int employerId, string firstname = "", string lastname = "", string ssn = "");

        Task<GenericResponse<EmployeeDto>> CreateEmployee(CreateEmployeeDto dto);

        Task<GenericResponse<EmployeeDto>> UpdateEmployee(int id, CreateEmployeeDto dto);

        Task<GenericResponse<CreateEmployeeDto>> GetEmployee(int id);
    }
}