using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IEnrollmentsService
    {
        Task<GenericResponse<List<EnrollmentDto>>> GetUsersEnrollments(List<int> userIds);
        Task<GenericResponse<List<EnrollmentDto>>> GetEnrollmentsByEmployeeId(int employeeId, int employerId, byte[] encodedEmployerEmployee);
        Task<GenericResponse<List<EnrollmentDto>>> UpsertEnrollments(int employerId, int employeeId, byte[] encodedEmployerEmployee, List<UpsertEnrollmentDto> enrollments);
    }
}
