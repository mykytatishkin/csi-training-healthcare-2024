using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IInsurancePackageService
    {
        void CreateInsurancePackage(CreateInsurancePackageDto dto);
    }
}
