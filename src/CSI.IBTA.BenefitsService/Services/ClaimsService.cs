using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.DTOs.Errors;

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
            var claim = await _benefitsUnitOfWork.Claims.GetById(claimId);

            if (claim == null)
                return new GenericResponse<ClaimDto>(HttpErrors.ResourceNotFound, null);

            return new GenericResponse<ClaimDto>(null, _mapper.Map<ClaimDto>(claim));
        }

        public async Task<GenericResponse<ClaimDto>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto)
        {
            var claim = await _benefitsUnitOfWork.Claims.GetById(claimId);
            if (claim == null)
                return new GenericResponse<ClaimDto>(HttpErrors.ResourceNotFound, null);

            claim.PlanId = updateClaimDto.PlanId;
            claim.DateOfService = updateClaimDto.DateOfService;
            claim.Amount = updateClaimDto.Amount;
            _benefitsUnitOfWork.Claims.Upsert(claim);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<ClaimDto>(null, _mapper.Map<ClaimDto>(claim));
        }
    }
}
