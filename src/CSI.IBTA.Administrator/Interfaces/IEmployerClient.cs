using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IEmployerClient
    {
        Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id);
    }
}
