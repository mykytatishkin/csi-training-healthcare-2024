using AutoMapper;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using System.Numerics;

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
                    plan.PackageId,
                    plan.Package.EmployerId));

            CreateMap<PlanType, PlanTypeDto>()
                .ConstructUsing(planType => new PlanTypeDto(
                    planType.Id,
                    planType.Name));

            CreateMap<Package, InsurancePackageDto>()
                .ConstructUsing(x => new InsurancePackageDto(
                    x.Id,
                    x.Name,
                    x.Status,
                    x.Initialized == null && DateTime.UtcNow < x.PlanEnd,
                    x.Initialized == null || DateTime.UtcNow > x.PlanEnd,
                    x.Initialized != null || DateTime.UtcNow > x.PlanEnd));

            CreateMap<Package, FullInsurancePackageDto>()
                .ConstructUsing(x => new FullInsurancePackageDto(
                    x.Id,
                    x.Name,
                    x.Initialized != null,
                    x.PlanStart,
                    x.PlanEnd,
                    x.PayrollFrequency,
                    x.EmployerId,
                    x.Plans.Select(plan => new PlanDto(
                        plan.Id,
                        plan.Name,
                        new PlanTypeDto(plan.PlanType.Id, plan.PlanType.Name),
                        plan.Contribution,
                        plan.PackageId,
                        plan.Package.EmployerId)).ToList()));

            CreateMap<Plan, UpdatePlanDto>()
                .ConstructUsing(plan => new UpdatePlanDto(
                    plan.Name,
                    plan.Contribution,
                    new PlanTypeDto(plan.PlanType.Id, plan.PlanType.Name)));


            CreateMap<Claim, ClaimDto>()
                .ConstructUsing(x => new ClaimDto(
                    x.Id,
                    x.Enrollment.EmployeeId,
                    x.Enrollment.Plan.Package.EmployerId,
                    x.Enrollment.PlanId,
                    x.ClaimNumber,
                    x.DateOfService,
                    x.Enrollment.Plan.Name,
                    x.Enrollment.Plan.PlanType.Name,
                    x.Amount,
                    x.Status,
                    x.RejectionReason)
                );

            CreateMap<Enrollment, EnrollmentDto>()
               .ConstructUsing(x => new EnrollmentDto(
                   x.Id,
                   x.PlanId,
                   x.Election,
                   x.Plan.Contribution,
                   x.EmployeeId)
               );

            //CreateMap<Enrollment, FullEnrollmentWithBalanceDto>()
            //    .ConstructUsing(x => new FullEnrollmentWithBalanceDto(
            //        x.Id,
            //        new PlanDto(
            //            x.Plan.Id,
            //            x.Plan.Name,
            //            new PlanTypeDto(x.Plan.PlanType.Id, x.Plan.PlanType.Name),
            //            x.Plan.Contribution,
            //            x.Plan.PackageId,
            //            x.Plan.Package.EmployerId),
            //        x.Election,
            //        x.Plan.Contribution,
            //        x.EmployeeId)
            //    );
        }
    }
}