using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using CSI.IBTA.Shared.Entities;

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

        public async Task<GenericResponse<PagedClaimsResponse>> GetClaims(int page, int pageSize, string claimNumber = "", string employerId = "", string claimStatus = "")
        {
            var filteredClaims = _benefitsUnitOfWork.Claims.GetSet()
                .Include(c => c.Plan.Package)
                .Include(c => c.Plan.PlanType)
                .Where(c => claimNumber != "" ? c.ClaimNumber.Contains(claimNumber) : true)
                .Where(c => employerId != "" ? c.Plan.Package.EmployerId.ToString() == employerId : true)
                .Where(c => claimStatus != "" ? c.Status == Enum.Parse<ClaimStatus>(claimStatus) : true);
         
            var totalCount = filteredClaims.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var claims = await filteredClaims
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var claimDtos = claims.Select(_mapper.Map<ClaimDto>).ToList();

            var response = new PagedClaimsResponse(claimDtos, page, pageSize, totalPages, totalCount);
            return new GenericResponse<PagedClaimsResponse>(null, response);
        }
    }
}
