using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IEnrollmentsClient
    {
        Task<GenericResponse<IEnumerable<EnrollmentDto>>> GetEnrollmentsByUserIds(List<int> userIds);
    }
}
