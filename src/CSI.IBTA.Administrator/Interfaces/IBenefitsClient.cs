using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IBenefitsClient
    {
        Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id);
    }
}
