using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IEnrollmentsService
    {
        Task<GenericResponse<List<EnrollmentDto>>> GetEnrollmentsByEmployeeId(int employeeId);
        Task<GenericResponse<List<EnrollmentDto>>> UpdateEnrollments(int employerId, int employeeId, List<UpsertEnrollmentDto> enrollments);
    }
}
