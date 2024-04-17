using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IClaimsClient
    {
        Task<GenericResponse<PagedClaimsResponse>> GetClaims(int page, int pageSize, string claimNumber = "", string employerId = "", string claimStatus = "");
        Task<GenericResponse<ClaimWithBalanceDto?>> GetClaim(int claimId);
        Task<GenericResponse<bool>> ApproveClaim(int claimId);
        Task<GenericResponse<bool>> DenyClaim(int claimId, DenyClaimDto dto);
        Task<GenericResponse<bool>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto);
    }
}
