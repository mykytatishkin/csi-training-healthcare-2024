using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IInsurancePackageService
    {
        Task<GenericHttpResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);
    }
}