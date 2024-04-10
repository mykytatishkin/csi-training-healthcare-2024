using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
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

        public async Task<GenericResponse<decimal>> GetCurrentBalance(int enrollmentId)
        {
            var enrollment = await _benefitsUnitOfWork.Enrollments
                .Include(c => c.Plan)
                .Include(c => c.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == enrollmentId);

            if (enrollment == null) return new GenericResponse<decimal>(HttpErrors.ResourceNotFound, 0);

            var package = enrollment.Plan.Package;
            if (!package.IsActive) return new GenericResponse<decimal>(new HttpError("This package is not active yet", HttpStatusCode.BadRequest), 0);

            var transactions = await _benefitsUnitOfWork.Transactions
                .Include(x => x.Enrollment)
                .Where(x => x.Enrollment.Id == enrollmentId)
                .ToListAsync();

            var balance = transactions.Sum(x => x.Type == TransactionType.Income ? x.Amount : -x.Amount);
            return new GenericResponse<decimal>(null, balance);
        }

        public async Task<GenericResponse<Dictionary<int, decimal>>> GetCurrentBalances(List<int> enrollmentIds)
        {
            Dictionary<int, decimal> balances = [];

            var distinctEnrollmentIds = enrollmentIds.Distinct();

            var enrollments = await _benefitsUnitOfWork.Enrollments
                .Include(c => c.Plan)
                .Include(c => c.Plan.Package)
                .Where(x => distinctEnrollmentIds.Contains(x.Id))
                .ToListAsync();

            foreach (var enrollment in enrollments)
            {
                var package = enrollment.Plan.Package;

                if (!package.IsActive)
                {
                    var error = new HttpError("One of the packages is not active", HttpStatusCode.BadRequest);
                    return new GenericResponse<Dictionary<int, decimal>>(error, null);
                }

                var transactions = await _benefitsUnitOfWork.Transactions
                    .Include(x => x.Enrollment)
                    .Where(x => x.Enrollment.Id == enrollment.Id)
                    .ToListAsync();

                var balance = transactions.Sum(x => x.Type == TransactionType.Income ? x.Amount : -x.Amount);
                balances.Add(enrollment.Id, balance);
            }

            return new GenericResponse<Dictionary<int, decimal>>(null, balances);
        }
    }
}
