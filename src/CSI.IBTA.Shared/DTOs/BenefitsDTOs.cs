namespace CSI.IBTA.Shared.DTOs
{
    public record PlanDto(int Id, string PlanTypeName, decimal Contribution, int PackageId);
    public record CreatePlanDto(decimal Contribution, string Status, int PackageId, int PlanTypeId, int EmployeeId);
}
