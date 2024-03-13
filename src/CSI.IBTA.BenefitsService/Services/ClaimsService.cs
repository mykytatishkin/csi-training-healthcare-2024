using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using System.Net;
using System.Numerics;

namespace CSI.IBTA.BenefitsService.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;

        public ClaimsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<List<ClaimDto>>> GetClaims()
        {
            var response = await _benefitsUnitOfWork.Claims
                .Include(c => c.Plan.Package)
                .Include(c => c.Plan.PlanType)
                .ToListAsync();

            return new GenericResponse<List<ClaimDto>>(null, response.Select(_mapper.Map<ClaimDto>).ToList());
        }

        public async Task<GenericResponse<ClaimDto>> GetClaim(int claimId)
        {
            var claim = await _benefitsUnitOfWork.Claims
                .Include(x => x.Plan)
                .Include(x => x.Plan.PlanType)
                .Include(c => c.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == claimId);

            if (claim == null) return new GenericResponse<ClaimDto>(HttpErrors.ResourceNotFound, null);

            return new GenericResponse<ClaimDto>(null, _mapper.Map<ClaimDto>(claim));
        }

        public async Task<GenericResponse<bool>> ApproveClaim(int claimId)
        {
            var claim = await _benefitsUnitOfWork.Claims
                .Include(x => x.Plan)
                .Include(c => c.Plan.Package)
                .FirstOrDefaultAsync(x => x.Id == claimId);

            if (claim == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            if (await GetCurrentBalance(claim.Plan) < claim.Amount)
            {
                return new GenericResponse<bool>(new HttpError("Consumer's balance is insufficient", HttpStatusCode.BadRequest), false);
            }

            claim.Status = ClaimStatus.Approved;
            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();
            return new GenericResponse<bool>(null, true);
        }

        private async Task<decimal> GetCurrentBalance(Plan plan)
        {
            if (!plan.Package.IsActive) return 0;

            var package = plan.Package;
            var negativeTransactions = await _benefitsUnitOfWork.Claims
                .Find(x => x.PlanId == plan.Id && x.Status == ClaimStatus.Approved);

            var totalPeriods = 1;
            if (package.PayrollFrequency == PayrollFrequency.Weekly)
            {
                totalPeriods += (int)Math.Floor((package.PlanEnd - package.PlanStart).TotalDays / 7.0);
            }
            else if (package.PayrollFrequency == PayrollFrequency.Monthly)
            {
                totalPeriods += (package.PlanEnd.Year - package.PlanStart.Year) * 12 + package.PlanEnd.Month - package.PlanStart.Month;
            }

            int periodsPassed = 1; 
            if (package.PayrollFrequency == PayrollFrequency.Weekly)
            {
                periodsPassed += (int)Math.Floor((DateTime.Now - package.PlanStart).TotalDays / 7.0);
            }
            else if (package.PayrollFrequency == PayrollFrequency.Monthly)
            {
                periodsPassed += (DateTime.UtcNow.Year - package.PlanStart.Year) * 12 + DateTime.UtcNow.Month - package.PlanStart.Month;
            }

            decimal amountPerPeriod = plan.Contribution / totalPeriods;
            var positiveSum = amountPerPeriod * periodsPassed;
            var negativeSum = negativeTransactions.Sum(x => x.Amount);
            return positiveSum - negativeSum;
        }

        public async Task<GenericResponse<bool>> DenyClaim(int claimId, DenyClaimDto dto)
        {
            var claim = await _benefitsUnitOfWork.Claims.GetById(claimId);

            if (claim == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);
            claim.Status = ClaimStatus.Denied;
            claim.RejectionReason = dto.RejectionReason;

            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<bool>(null, true);
        }
    }
}
