using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IEnrollmentsClient
    {
        Task<GenericResponse<IEnumerable<EnrollmentDto>>> GetEnrollmentsByUserIds(List<int> userIds);
        Task<GenericResponse<List<EnrollmentDto>>> GetEmployeeEnrollments(int employeeId, GetEnrollmentsDto dto);
        Task<GenericResponse<List<FullInsurancePackageDto>>> GetEmployerPackages(int employerId);
        Task<GenericResponse<List<EnrollmentDto>>> UpdateEnrollments(int employerId, int employeeId, UpsertEnrollmentsDto updateEnrollmentsDto);
    }
}