using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IClaimsService
    {
        Task<GenericResponse<List<ClaimDto>>> GetClaims();
        Task<GenericResponse<ClaimDetailsDto>> GetClaimDetails(int claimId);
    }
}
