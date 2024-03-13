using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs
{
    public record ClaimDto(
        int Id,
        int EmployeeId,
        int EmployerId,
        string ClaimNumber,
        DateOnly DateOfService,
        string PlanName,
        string PlanTypeName,
        decimal Amount,
        ClaimStatus Status,
        string? RejectionReason);

    public record DenyClaimDto(string RejectionReason);
}