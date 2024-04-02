using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IPlansClient
    {
        Task<GenericResponse<IEnumerable<PlanDto>>> GetPlansByNames(List<string> planNames);
    }
}
