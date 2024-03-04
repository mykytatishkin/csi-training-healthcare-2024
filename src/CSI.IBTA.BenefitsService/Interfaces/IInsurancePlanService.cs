using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IInsurancePlanService
    {
        public Task<GenericHttpResponse<IEnumerable<PlanDto>>> GetAllPlans();
        public Task<GenericHttpResponse<PlanDto>> GetPlan(int planId);
        public Task<GenericHttpResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();
        public Task<GenericHttpResponse<PlanDto>> CreatePlan(int packageId, CreatePlanDto createPlanDto);
        public Task<GenericHttpResponse<PlanDto>> UpdatePlan(int planId, UpdatePlanDto updatePlanDto);
    }
}