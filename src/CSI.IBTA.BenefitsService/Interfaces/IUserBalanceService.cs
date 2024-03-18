
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IUserBalanceService
    {
        Task<GenericResponse<decimal>> GetCurrentBalance(int enrollmentId);
    }
}
