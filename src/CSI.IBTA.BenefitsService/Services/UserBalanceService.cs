using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class UserBalanceService : IUserBalanceService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;

        public UserBalanceService(IBenefitsUnitOfWork benefitsUnitOfWork)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
        }

        public async Task<GenericResponse<decimal>> GetCurrentBalanceForPlan(int planId)
        {
            var plan = await _benefitsUnitOfWork.Plans
                .Include(c => c.Package)
                .FirstOrDefaultAsync(x => x.Id == planId);

            if(plan == null) return new GenericResponse<decimal>(HttpErrors.ResourceNotFound, 0);
            if(!plan.Package.IsActive) return new GenericResponse<decimal>(new HttpError("This plan is not active yet", HttpStatusCode.BadRequest), 0);

            var package = plan.Package;
            var negativeTransactions = await _benefitsUnitOfWork.Claims
                .Find(x => x.PlanId == plan.Id && x.Status == ClaimStatus.Approved);

            var totalPeriods = 0;
            int periodsPassed = 0;
            switch (package.PayrollFrequency) 
            {
                case PayrollFrequency.Weekly:
                    totalPeriods += (int)Math.Ceiling((package.PlanEnd - package.PlanStart).TotalDays / 7.0);

                    periodsPassed += (int)Math.Ceiling((DateTime.UtcNow - package.PlanStart).TotalDays / 7.0);
                    break;
                case PayrollFrequency.Monthly:
                    totalPeriods += (package.PlanEnd.Year - package.PlanStart.Year) * 12; //add years
                    totalPeriods += package.PlanEnd.Month - package.PlanStart.Month; //add months
                    totalPeriods += package.PlanEnd.Day >= package.PlanStart.Day ? 1 : 0; //check if there are month remains

                    periodsPassed += (DateTime.UtcNow.Year - package.PlanStart.Year) * 12; //add years
                    periodsPassed += DateTime.UtcNow.Month - package.PlanStart.Month; //add months
                    periodsPassed += DateTime.UtcNow.Day >= package.PlanStart.Day ? 1 : 0; //check if there are month remains
                    break;
                default:
                    throw new NotImplementedException();
            }

            decimal amountPerPeriod = plan.Contribution / totalPeriods;
            var positiveSum = amountPerPeriod * periodsPassed;
            var negativeSum = negativeTransactions.Sum(x => x.Amount);
            return new GenericResponse<decimal>(null, positiveSum - negativeSum);
        }
    }
}
