using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.BenefitsService.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;

        public ClaimService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
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
