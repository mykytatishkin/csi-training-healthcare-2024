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
        Task<GenericResponse<byte[]>> GetEncryptedEmployee(int employerId, int employeeId);

        Task<GenericResponse<FullEmployeeDto?>> CreateEmployee(CreateEmployeeDto command);

        Task<GenericResponse<FullEmployeeDto>> GetEmployee(int id);

        Task<GenericResponse<bool?>> UpdateEmployee(UpdateEmployeeDto command);
    }
}