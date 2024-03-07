using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IInsurancePackageService
    {
        Task<GenericResponse<CreatedInsurancePackageDto>> CreateInsurancePackage(CreateInsurancePackageDto dto);

        Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);

        Task<GenericResponse<FullInsurancePackageDto>> GetInsurancePackage(int packageId);
    }
}