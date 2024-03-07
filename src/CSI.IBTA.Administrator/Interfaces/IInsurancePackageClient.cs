using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IInsurancePackageClient
    {
        Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command);

        Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();

        Task<GenericResponse<bool?>> UpdateInsurancePackage(FullInsurancePackageDto command);

        Task<GenericResponse<FullInsurancePackageDto>> GetInsurancePackage(int insurancePackageId);
    }
}