using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.ProcessingService.Constants;
using CSI.IBTA.ProcessingService.Interfaces;
using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.ProcessingService.Services
{
    public sealed class ScopedProcessingService(
        ILogger<ScopedProcessingService> _logger,
        IBenefitsUnitOfWork _benefitsUnitOfWork) : IScopedProcessingService
    {
        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                var enrollments = await _benefitsUnitOfWork.Enrollments.GetSet()
                    .Include(e => e.Plan.Package)
                    .Where(e => e.Plan.Package.Initialized != null)
                    .Where(e => e.Plan.Package.PlanStart <= DateTime.UtcNow && e.Plan.Package.PlanEnd >= DateTime.UtcNow)
                    .ToListAsync();

                foreach (Enrollment enrollment in enrollments)
                {
                    var transactionCount = await _benefitsUnitOfWork.Transactions.GetSet()
                        .Where(t => t.EnrollmentId == enrollment.Id)
                        .Where(t => t.Type == TransactionType.Income)
                        .CountAsync(cancellationToken: stoppingToken);

                    Plan plan = enrollment.Plan;
                    Package package = plan.Package;
                    DateTime startDate = package.PlanStart;
                    DateTime endDate = package.PlanEnd;
                    PayrollFrequency payrollFrequency = package.PayrollFrequency;

                    var payrollFrequencyTimeSpan = payrollFrequency == PayrollFrequency.Weekly
                        ? new TimeSpan(7, 0, 0, 0)
                        : new TimeSpan(30, 0, 0, 0);
                    
                    TimeSpan packageDuration = endDate - startDate;
                    int totalNumberOfPays = (int)Math.Floor(packageDuration.TotalHours / payrollFrequencyTimeSpan.TotalHours) + 1;

                    TimeSpan timePassed = DateTime.UtcNow - startDate;
                    int numberOfPays = (int)Math.Floor(timePassed.TotalHours / payrollFrequencyTimeSpan.TotalHours) + 1;
                    int transactionCountDifference = numberOfPays - transactionCount;

                    decimal totalInsuranceAmount = plan.Contribution + enrollment.Election;
                    decimal frequencyTransactionAmount = totalInsuranceAmount / (int)packageDuration.TotalDays * (int)payrollFrequencyTimeSpan.TotalDays;
                    
                    // Handles situations if system was down and missed payments
                    for (int i = 0; i < transactionCountDifference; i++)
                    {
                        transactionCount++;

                        decimal transactionAmount = transactionCount == totalNumberOfPays
                            ? totalInsuranceAmount - (numberOfPays - 1) * frequencyTransactionAmount
                            : frequencyTransactionAmount;

                        var transaction = new Transaction
                        {
                            Amount = transactionAmount,
                            DateTime = DateTime.UtcNow,
                            Enrollment = enrollment,
                            Type = TransactionType.Income
                        };

                        await _benefitsUnitOfWork.Transactions.Add(transaction);
                    }
                }

                await _benefitsUnitOfWork.CompleteAsync();

                await Task.Delay(WorkerConfigConstants.CHECK_INTERVAL_MS, stoppingToken);
            }
        }
    }
}
