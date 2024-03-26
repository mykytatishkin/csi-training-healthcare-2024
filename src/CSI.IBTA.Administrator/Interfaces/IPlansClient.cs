using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IPlansClient
    {
        Task<GenericResponse<List<PlanDto>>> GetPlans(int? userId = null);
        Task<GenericResponse<PlanDto>> GetPlan(int planId);
        Task<GenericResponse<List<PlanTypeDto>>> GetPlanTypes();
        Task<GenericResponse<bool?>> CreatePlan(CreatePlanDto model);
        Task<GenericResponse<bool?>> UpdatePlan(int planId, UpdatePlanDto planDto);
    }
}
