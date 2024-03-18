using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IPlansClient
    {
        Task<GenericResponse<PlanDto>> GetPlan(int id);
        Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();
        Task<GenericResponse<bool?>> CreatePlan(CreatePlanDto model);
        Task<GenericResponse<bool?>> UpdatePlan(int planId, UpdatePlanDto planDto);
    }
}
