using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Types
{
    public class ContributionsResponse
    {
        public List<ProcessedContributionDto> ProcessedContributions { get; set; } = [];
        public Dictionary<int, List<string>> Errors { get; set; } = [];
    }
}
