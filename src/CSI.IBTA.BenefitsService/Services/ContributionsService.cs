using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.BenefitsService.Services
{
    public class ContributionsService : IContributionsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;

        public ContributionsService(IBenefitsUnitOfWork benefitsUnitOfWork)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
        }

        public async Task<GenericResponse<bool>> CreateContributions(List<ProcessedContributionDto> processedContributions)
        {
            foreach (var contribution in processedContributions)
            {
                Transaction transaction = new()
                {
                    EnrollmentId = contribution.TransactionId,
                    Amount = contribution.Contribution,
                    DateTime = DateTime.UtcNow,
                    Reason = TransactionReason.Bonus,
                    Type = TransactionType.Income
                };

                await _benefitsUnitOfWork.Transactions.Add(transaction);
            }

            await _benefitsUnitOfWork.CompleteAsync();

            return new(null, true);
        }
    }
}
