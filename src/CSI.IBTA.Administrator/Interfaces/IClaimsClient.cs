using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IClaimsClient
    {
        Task<GenericResponse<IQueryable<ClaimDto>?>> GetClaims();
        Task<GenericResponse<ClaimDto?>> GetClaimDetails(int claimId);
    }
}
