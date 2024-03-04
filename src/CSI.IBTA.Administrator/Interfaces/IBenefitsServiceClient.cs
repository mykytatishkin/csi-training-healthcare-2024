using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IBenefitsServiceClient
    {
        Task<GenericHttpResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);
    }
}
