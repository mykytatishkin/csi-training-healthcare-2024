using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Interfaces
{
    public interface IEnrollmentsClient
    {
        Task<GenericResponse<PagedEnrollmentsResponse>> GetEmployeeEnrollmentsPaged(int employeeId, GetEnrollmentsDto dto, int pageNumber, int pageSize);
    }
}