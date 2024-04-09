using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IContributionsClient
    {
        Task<GenericResponse<bool?>> CreateContributions(List<ProcessedContributionDto> processedContributions);
    }
}
