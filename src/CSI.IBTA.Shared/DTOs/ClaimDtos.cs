
namespace CSI.IBTA.Shared.DTOs
{
    public record ClaimDetailsDto(int Id, string Number, DateOnly DateOfService, string PlanName, decimal Amount);
}
