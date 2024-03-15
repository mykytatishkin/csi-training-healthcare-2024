using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IInsurancePackageClient
    {
        Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command);

        Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();

        Task<GenericResponse<bool?>> UpdateInsurancePackage(UpdateInsurancePackageDto command);

        Task<GenericResponse<FullInsurancePackageDto>> GetInsurancePackage(int insurancePackageId);
    }
}