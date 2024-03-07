using AutoMapper;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.BenefitsService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Package, InsurancePackageDto>()
                .ConstructUsing(x => new InsurancePackageDto(
                    x.Id,
                    x.Name,
                    x.Status,
                    DateTime.UtcNow < x.PlanEnd,
                    x.Initialized == null || DateTime.UtcNow > x.PlanEnd,
                    x.Initialized != null)
                );

            CreateMap<Claim, ClaimDto>()
                .ConstructUsing(x => new ClaimDto(
                    x.Id,
                    x.EmployeeId,
                    x.Plan.Package.EmployerId,
                    x.ClaimNumber,
                    x.DateOfService,
                    x.Plan.PlanType.Name,
                    x.Amount,
                    x.Status));
        }
    }
}