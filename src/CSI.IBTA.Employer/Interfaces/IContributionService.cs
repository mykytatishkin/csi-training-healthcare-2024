using CSI.IBTA.Employer.Types;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IContributionService
    {
        Task<GenericResponse<ContributionsResponse>> ProcessContributionsFile(IFormFile file);
    }
}
