namespace CSI.IBTA.Shared.DTOs
{
    public record UnprocessedContributionDto(int RecordNumber, string SSN, string PlanName, decimal Contribution);
    public record ProcessedContributionDto(int EmployeeId, int PlanId, decimal Contribution);
}
