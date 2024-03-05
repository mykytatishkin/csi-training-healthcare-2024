using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IInsurancePlanService
    {
        public Task<GenericResponse<IEnumerable<PlanDto>>> GetAllPlans();
        public Task<GenericResponse<PlanDto>> GetPlan(int planId);
        public Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();
        public Task<GenericResponse<PlanDto>> CreatePlan(int packageId, CreatePlanDto createPlanDto);
        public Task<GenericResponse<PlanDto>> UpdatePlan(int planId, UpdatePlanDto updatePlanDto);
    }
}