using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IBenefitsServiceClient
    {
        Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);
        Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId);
        Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId);
        Task<GenericResponse<ClaimDto>> GetClaim(int claimId);
        Task<GenericResponse<ClaimDto>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto);
        Task<GenericResponse<List<PlanIdAndNameDto>>> GetPlans();
        Task<GenericResponse<PlanIdAndNameDto>> GetPlan(int planId);
    }
}
