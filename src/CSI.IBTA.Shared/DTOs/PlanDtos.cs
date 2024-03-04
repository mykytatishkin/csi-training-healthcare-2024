namespace CSI.IBTA.Shared.DTOs
{
    public record CreatedPlanDto(int Id, string Name, int PlanTypeId, decimal Contribution);
    public record CreatePlanDto(string Name, int PlanTypeId, decimal Contribution);
}