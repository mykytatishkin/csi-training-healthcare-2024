using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IContributionsService
    {
        Task<GenericResponse<bool>> CreateContributions(List<ProcessedContributionDto> processedContributions, int employerId);
    }
}
