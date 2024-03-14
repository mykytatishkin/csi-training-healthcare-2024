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

    public record PagedClaimsResponse(
        List<ClaimDto> Claims,
        int CurrentPage,
        int PageSize,
        int TotalPages,
        int TotalCount);

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
}