using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IClaimsClient
    {
        Task<GenericResponse<ClaimDto?>> GetClaimDetails(int claimId);
        Task<GenericResponse<bool>> ApproveClaim(int claimId);
        Task<GenericResponse<bool>> DenyClaim(int claimId, DenyClaimDto dto);
        Task<GenericResponse<PagedClaimsResponse>> GetClaims(int page, int pageSize, string claimNumber = "", string employerId = "", string claimStatus = "");
    }
}
