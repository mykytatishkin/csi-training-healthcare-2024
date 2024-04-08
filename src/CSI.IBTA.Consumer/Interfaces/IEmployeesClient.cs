using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Consumer.Interfaces
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
    }
}
