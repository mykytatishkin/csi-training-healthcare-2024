using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IBenefitsClient
    {
        Task<GenericInternalResponse<PlanDto>> GetPlan(int id);
        Task<GenericInternalResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes();
        Task<GenericInternalResponse<bool?>> CreatePlan(CreatePlanDto model);
        Task<GenericInternalResponse<bool?>> UpdatePlan(int planId, UpdatePlanDto planDto);
    }
}
