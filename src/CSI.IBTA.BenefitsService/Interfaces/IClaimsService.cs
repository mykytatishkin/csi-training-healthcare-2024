using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IClaimsService
    {
        Task<GenericResponse<ClaimWithBalanceDto>> GetClaim(int claimId);
        Task<GenericResponse<bool>> ApproveClaim(int claimId);
        Task<GenericResponse<bool>> DenyClaim(int claimId, DenyClaimDto dto);
        Task<GenericResponse<PagedClaimsResponse>> GetClaims(int page, int pageSize, string claimNumber = "", string employerId = "", string employeeId = "", string claimStatus = "");
        Task<GenericResponse<bool>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto);
    }
}
