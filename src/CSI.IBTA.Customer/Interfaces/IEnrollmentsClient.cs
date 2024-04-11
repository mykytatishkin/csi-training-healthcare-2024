using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Interfaces
{
    public interface IEnrollmentsClient
    {
        Task<GenericResponse<IEnumerable<EnrollmentDto>>> GetEnrollmentsByUserIds(List<int> userIds);
        Task<GenericResponse<List<EnrollmentDto>>> GetEmployeeEnrollments(int employeeId, GetEnrollmentsDto dto);
        Task<GenericResponse<PagedEnrollmentsResponse>> GetEmployeeEnrollmentsPaged(int employeeId, GetEnrollmentsDto dto, int pageNumber, int pageSize);
        Task<GenericResponse<List<FullInsurancePackageDto>>> GetEmployerPackages(int employerId);
        Task<GenericResponse<List<EnrollmentDto>>> UpdateEnrollments(int employerId, int employeeId, UpsertEnrollmentsDto updateEnrollmentsDto);
        Task<GenericResponse<Dictionary<int, decimal>>> GetEnrollmentsBalances(List<int> enrollmentsIds);
    }
}