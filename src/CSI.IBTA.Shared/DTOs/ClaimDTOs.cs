using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Shared.DTOs
{
    public record ClaimDto(
        int Id,
        int EmployeeId,
        int EmployerId,
        int PlanId,
        string ClaimNumber,
        DateOnly DateOfService,
        string PlanName,
        string PlanTypeName,
        decimal Amount,
        ClaimStatus Status);

    public record ViewClaimDto(
        int Id,
        int EmployeeId,
        string EmployeeName,
        int EmployerId,
        string EmployerName,
        string ClaimNumber,
        DateOnly DateOfService,
        string PlanTypeName,
        decimal Amount,
        ClaimStatus Status);

    public record UpdateClaimDto(DateOnly DateOfService, int PlanId, decimal Amount);
}