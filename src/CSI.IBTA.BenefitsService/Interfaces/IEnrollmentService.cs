using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IEnrollmentService
    {
        Task<GenericResponse<List<EnrollmentDto>>> GetUsersEnrollments(List<int> userIds);
    }
}
