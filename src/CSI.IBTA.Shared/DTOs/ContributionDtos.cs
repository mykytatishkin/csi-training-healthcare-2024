namespace CSI.IBTA.Shared.DTOs
{
    public record UnprocessedContributionDto(int RecordNumber, string Username, string PlanName, decimal Contribution);
    public record ProcessedContributionDto(int EnrollmentId, decimal Contribution);
}
