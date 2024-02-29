using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs
{
    public record CreateInsurancePackageDto(string Name, DateOnly PlanStart, DateOnly PlanEnd, string PayrollFrequency, List<Plan> Plans);
}
