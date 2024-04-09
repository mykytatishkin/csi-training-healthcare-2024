using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
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

        public async Task<GenericResponse<bool>> CreateContributions(List<ProcessedContributionDto> processedContributions, int employerId)
        {
            if (processedContributions.Count == 0)
            {
                return new(null, true);
            }

            var enrollmentIds = processedContributions
                .Select(c => c.EnrollmentId)
                .ToList();

            var employersCount = _benefitsUnitOfWork.Enrollments
                .Include(e => e.Plan.Package)
                .Where(e => enrollmentIds.Contains(e.Id))
                .GroupBy(e => e.Plan.Package.EmployerId)
                .Select(group => group.Key)
                .Count();

            if (employersCount > 1)
            {
                var error = new HttpError("Contributions contain data from more than one employer", System.Net.HttpStatusCode.BadRequest);
                return new(error, false);
            }

            var transactionsEmployerId = _benefitsUnitOfWork.Enrollments
                .Include(e => e.Plan.Package)
                .Where(e => e.Id == processedContributions.First().EnrollmentId)
                .Select(e => e.Plan.Package.EmployerId)
                .FirstOrDefault();

            if (transactionsEmployerId != employerId)
            {
                var error = new HttpError("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
                return new(error, false);
            }

            foreach (var contribution in processedContributions)
            {
                Transaction transaction = new()
                {
                    EnrollmentId = contribution.EnrollmentId,
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
