using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IBenefitsServiceClient
    {
        Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId);
    }
}
