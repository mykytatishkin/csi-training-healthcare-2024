using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IUserValidationService
    {
        Task<GenericResponse<bool>> DoesEmployeeBelongToEmployer(int employerId, int employeeId);
    }
}
