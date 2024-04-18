using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployeesService
    {
        Task<GenericResponse<PagedEmployeesResponse>> GetEmployees(int page, int pageSize, int employerId, string firstname = "", string lastname = "", string ssn = "");

        Task<GenericResponse<FullEmployeeDto>> CreateEmployee(CreateEmployeeDto dto);

        Task<GenericResponse<FullEmployeeDto>> UpdateEmployee(UpdateEmployeeDto dto);

        Task<GenericResponse<FullEmployeeDto>> GetEmployee(int id);

        public Task<GenericResponse<IEnumerable<UserDto>>> GetEmployeesByUsernames(List<string> usernames, int employerId);

        public Task<GenericResponse<bool>> GetAllowClaimFilling(int employeeId);
    }
}