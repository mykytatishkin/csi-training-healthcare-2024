using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs
{
    public record InsurancePackageDto(int Id, string Name, string Status, bool CanBeModified, bool CanBeRemoved, bool IsInitialized);
    
    public record CreatedInsurancePackageDto(
        int Id,
        string Name,
        DateTime PlanStart,
        DateTime PlanEnd,
        PayrollFrequency PayrollFrequency,
        int EmployerId,
        List<CreatedPlanDto> Plans);

    public record CreateInsurancePackageDto(
        string Name,
        DateTime PlanStart,
        DateTime PlanEnd,
        PayrollFrequency PayrollFrequency,
        int EmployerId,
        List<CreatePlanDto> Plans);
}
