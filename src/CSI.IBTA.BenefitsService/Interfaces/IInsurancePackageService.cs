using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IInsurancePackageService
    {
        Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);
        Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId);
        Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId);
    }
}