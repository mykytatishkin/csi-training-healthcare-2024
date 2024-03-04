using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs
{
    public record CreatedInsurancePackageDto(
        int Id,
        string Name,
        DateOnly PlanStart,
        DateOnly PlanEnd,
        PayrollFrequency PayrollFrequency,
        int EmployerId,
        List<CreatedPlanDto> Plans);

    public record CreateInsurancePackageDto(
        string Name,
        DateOnly PlanStart,
        DateOnly PlanEnd,
        PayrollFrequency PayrollFrequency,
        int EmployerId,
        List<CreatePlanDto> Plans);
}
