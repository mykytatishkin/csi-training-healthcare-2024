namespace CSI.IBTA.Shared.DTOs
{
    public record PlanTypeDto(int Id, string Name);
    public record PlanDto(int Id, string Name, PlanTypeDto PlanType, decimal Contribution, int PackageId);
    public record CreatePlanDto(string Name, decimal Contribution, int PlanTypeId);
    public record UpdatePlanDto(string Name, decimal Contribution, int PlanTypeId);
}
