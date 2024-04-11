using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Interfaces
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
        Task<GenericResponse<byte[]>> GetEncryptedEmployee(int employerId, int employeeId);
        Task<GenericResponse<bool?>> CreateEmployee(CreateEmployeeDto command);
        Task<GenericResponse<UserDto>> GetEmployee(int userId);
        Task<GenericResponse<IEnumerable<UserDto>>> GetEmployeesByUsernames(List<string> usernames, int employerId);
        Task<GenericResponse<FullEmployeeDto>> GetEmployee(int id);
        Task<GenericResponse<string?>> GetEmployerLogo();
    }
}