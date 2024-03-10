using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
    }
}
