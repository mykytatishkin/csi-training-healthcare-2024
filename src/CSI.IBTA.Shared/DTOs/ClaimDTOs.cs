using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Http;

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
        ClaimStatus Status,
        string? RejectionReason,
        string Receipt);

    public record ClaimShortDto(
        int Id,
        int EmployeeId,
        int EmployerId,
        int PlanId,
        string ClaimNumber,
        DateOnly DateOfService,
        string PlanName,
        string PlanTypeName,
        decimal Amount,
        ClaimStatus Status,
        string? RejectionReason);

    public record ClaimWithBalanceDto(
        ClaimDto Claim,
        decimal EnrollmentBalance);

    public record DenyClaimDto(string RejectionReason);

    public record PagedClaimsResponse(
        List<ClaimShortDto> Claims,
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

    public record UpdateClaimDto(DateOnly DateOfService, int PlanId, decimal Amount);
    public record UploadFileClaimDto(DateOnly DateOfService, int EnrollmentId, decimal Amount, IFormFile Receipt, IFormFile EncryptedEmployerEmployeeSettings);
    public record FileClaimDto(DateOnly DateOfService, int EnrollmentId, decimal Amount, IFormFile Receipt, byte[] EncryptedEmployerEmployeeSettings);
}