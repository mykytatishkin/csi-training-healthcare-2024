using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IClaimsService
    {
        Task<GenericResponse<List<ClaimDto>>> GetClaims();
        Task<GenericResponse<ClaimDto>> GetClaim(int claimId);
        Task<GenericResponse<bool>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto);
    }
}
