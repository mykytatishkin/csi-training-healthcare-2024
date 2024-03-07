using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IInsurancePackageClient
    {
        Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command);
        Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();
    }
}
