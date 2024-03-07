using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;
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

        public async Task<GenericResponse<ClaimDetailsDto>> GetClaimDetails(int claimId)
        {
            var claim = await _benefitsUnitOfWork.Claims.Include(x => x.Plan).FirstOrDefaultAsync(x => x.Id == claimId);
            if (claim == null) return new GenericResponse<ClaimDetailsDto>(HttpErrors.ResourceNotFound, null);

            return new GenericResponse<ClaimDetailsDto>(null, _mapper.Map<ClaimDetailsDto>(claim));
        }

        public async Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var packages = await _benefitsUnitOfWork.Packages.Find(x => x.EmployerId == employerId && x.IsRemoved != true);

            return new GenericResponse<List<InsurancePackageDto>>(null, packages.Select(_mapper.Map<InsurancePackageDto>).ToList());
        }

    }
}
