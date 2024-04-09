using CSI.IBTA.ProcessingService.Interfaces;
using CSI.IBTA.ProcessingService.Constants;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.Entities;

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

                var currentDate = DateTime.UtcNow.Date;

                var enrollments = await _benefitsUnitOfWork.Enrollments.GetSet()
                    .Include(e => e.Plan.Package)
                    .Where(e => e.Plan.Package.Initialized != null)
                    .Where(e => e.Plan.Package.PlanStart <= currentDate && e.Plan.Package.PlanEnd >= currentDate)
                    .ToListAsync();

                foreach (Enrollment enrollment in enrollments)
                {
                    var transactionCount = await _benefitsUnitOfWork.Transactions.GetSet()
                        .Where(t => t.EnrollmentId == enrollment.Id)
                        .Where(t => t.Type == TransactionType.Income)
                        .Where(t => t.Reason == TransactionReason.Regular)
                        .CountAsync(cancellationToken: stoppingToken);

                    Plan plan = enrollment.Plan;
                    Package package = plan.Package;
                    DateTime startDate = package.PlanStart;
                    DateTime endDate = package.PlanEnd;
                    PayrollFrequency payrollFrequency = package.PayrollFrequency;

                    int payrollFrequencyDays = payrollFrequency == PayrollFrequency.Weekly ? 7 : 30;

                    int numberOfPays = CalculateNumberOfPayments(payrollFrequencyDays, startDate, endDate, DateTime.UtcNow);
                    int transactionCountDifference = numberOfPays - transactionCount;

                    decimal totalInsuranceAmount = plan.Contribution + enrollment.Election;
                    int totalNumberOfPays = CalculateNumberOfPayments(payrollFrequencyDays, startDate, endDate, endDate);
                    decimal transactionAmount = totalInsuranceAmount / totalNumberOfPays;

                    // Handles situations if system was down and missed payments
                    for (int i = 0; i < transactionCountDifference; i++)
                    {
                        var transaction = new Transaction
                        {
                            Amount = transactionAmount,
                            DateTime = DateTime.UtcNow,
                            Enrollment = enrollment,
                            Type = TransactionType.Income,
                            Reason = TransactionReason.Regular,
                        };

                        await _benefitsUnitOfWork.Transactions.Add(transaction);
                    }
                }

                await _benefitsUnitOfWork.CompleteAsync();

                await Task.Delay(WorkerConfigConstants.CHECK_INTERVAL_MS, stoppingToken);
            }
        }

        private int CalculateNumberOfPayments(int payrollFrequencyDays, DateTime startDate, DateTime endDate, DateTime currentDate)
        {
            TimeSpan timeSpanToNow = currentDate - startDate;
            int numberOfDaysToNow = (int)timeSpanToNow.TotalDays;
            int numberOfPaymentsToNow = numberOfDaysToNow / payrollFrequencyDays;

            TimeSpan timeSpanToEnd = endDate - startDate;
            int numberOfDaysToEnd = (int)timeSpanToEnd.TotalDays;
            int fullNumberOfPaymentsToEnd = numberOfDaysToEnd / payrollFrequencyDays;

            bool hasIntervalRemainder = (int)timeSpanToEnd.TotalDays % payrollFrequencyDays == 0;

            if (!hasIntervalRemainder &&
                numberOfPaymentsToNow == fullNumberOfPaymentsToEnd &&
                currentDate.Date == endDate.Date)
            {
                numberOfPaymentsToNow++;
            }

            return numberOfPaymentsToNow;
        }
    }
}
