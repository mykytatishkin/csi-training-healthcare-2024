using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IClaimService
    {
        Task<GenericResponse<ClaimDto>> GetClaim(int claimId);
        Task<GenericResponse<ClaimDto>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto);
    }
}