namespace CSI.IBTA.Shared.DTOs
{
    public record ClaimDetailsDto(
        int Id,
        string Number,
        DateOnly DateOfService,
        string PlanName,
        decimal Amount);

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