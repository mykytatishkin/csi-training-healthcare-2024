using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IClaimService
    {
        Task<GenericResponse<ClaimDetailsDto>> GetClaimDetails(int claimId);
    }
}