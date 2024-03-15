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
            var transactions = await _benefitsUnitOfWork.Transactions
                .Include(x => x.Enrollment)
                .Where(x => x.Enrollment.PlanId == planId)
                .ToListAsync();

            var balance = transactions.Sum(x => x.Type == TransactionType.Income ? x.Amount : - x.Amount);
            return new GenericResponse<decimal>(null, balance);
        }
    }
}
