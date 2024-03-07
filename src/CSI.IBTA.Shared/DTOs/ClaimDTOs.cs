namespace CSI.IBTA.Shared.DTOs
{
    public record ClaimDto(
        int Id,
        int EmployeeId,
        int EmployerId,
        string ClaimNumber,
        DateOnly DateOfService,
        string PlanTypeName,
        decimal Amount,
        string Status);
}