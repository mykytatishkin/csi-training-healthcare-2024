using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IInsurancePackageClient
    {
        Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command);

        Task<GenericResponse<bool?>> UpdateInsurancePackage(CreateInsurancePackageDto command);

        Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();

        Task<GenericResponse<int?>> GetInsurancePackage(int insurancePackageId);
    }
}