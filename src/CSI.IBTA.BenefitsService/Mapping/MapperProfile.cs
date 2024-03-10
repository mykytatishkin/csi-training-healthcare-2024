using AutoMapper;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.BenefitsService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Plan, PlanDto>()
                .ConstructUsing(plan => new PlanDto(
                    plan.Id,
                    plan.Name,
                    new PlanTypeDto(plan.PlanType.Id, plan.PlanType.Name),
                    plan.Contribution,
                    plan.PackageId));

            CreateMap<PlanType, PlanTypeDto>()
                .ConstructUsing(planType => new PlanTypeDto(
                    planType.Id,
                    planType.Name));

            CreateMap<Package, InsurancePackageDto>()
                .ConstructUsing(x => new InsurancePackageDto(
                    x.Id,
                    x.Name,
                    x.Status,
                    DateTime.UtcNow < x.PlanEnd,
                    x.Initialized == null || DateTime.UtcNow > x.PlanEnd,
                    x.Initialized != null || DateTime.UtcNow > x.PlanEnd));
            CreateMap<Claim, ClaimDto>()
                .ConstructUsing(claim => new ClaimDto(
                    claim.Id,
                    claim.EmployeeId,
                    claim.ClaimNumber,
                    claim.DateOfService,
                    claim.PlanId,
                    claim.Amount)
                );
        }
    }
}
