using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using System.Net;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class ClaimsService : IClaimsService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserBalanceService _userBalanceService;

        public ClaimsService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper, IUserBalanceService userBalanceService)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
            _userBalanceService = userBalanceService;
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
                .FirstOrDefaultAsync(x => x.Id == claimId);

            if (claim == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            var res = await _userBalanceService.GetCurrentBalanceForPlan(claim.Plan.Id);
            if (res.Error != null) return new GenericResponse<bool>(res.Error, false);

            if (res.Result < claim.Amount)
            {
                return new GenericResponse<bool>(new HttpError("Consumer's balance is insufficient", HttpStatusCode.BadRequest), false);
            }

            claim.Status = ClaimStatus.Approved;
            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();
            return new GenericResponse<bool>(null, true);
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
