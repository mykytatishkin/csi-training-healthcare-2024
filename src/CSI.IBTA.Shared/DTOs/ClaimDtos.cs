
namespace CSI.IBTA.Shared.DTOs
{
    public record ClaimDto(int Id, int EmployeeId, string ClaimNumber,  DateOnly DateOfService, int PlanId, decimal Amount);
    public record UpdateClaimDto(DateOnly DateOfService, int PlanId, decimal Amount);
}
