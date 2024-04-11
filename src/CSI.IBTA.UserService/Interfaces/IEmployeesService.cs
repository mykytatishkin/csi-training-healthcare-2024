using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployeesService
    {
        Task<GenericResponse<PagedEmployeesResponse>> GetEmployees(int page, int pageSize, int employerId, string firstname = "", string lastname = "", string ssn = "");
        Task<GenericResponse<FullEmployeeDto>> GetEmployee(int employeeId);
        public Task<GenericResponse<IEnumerable<UserDto>>> GetEmployeesByUsernames(List<string> usernames, int employerId);
        Task<GenericResponse<EmployeeDto>> CreateEmployee(CreateEmployeeDto dto);
    }
}