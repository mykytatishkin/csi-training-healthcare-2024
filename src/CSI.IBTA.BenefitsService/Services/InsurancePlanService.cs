using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.BenefitsService.Interfaces;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class InsurancePlanService : IInsurancePlanService
    {
        private readonly IBenefitsUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InsurancePlanService(IBenefitsUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericHttpResponse<IEnumerable<PlanDto>>> GetAllPlans()
        {
            var plans = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .ToListAsync();

            var planDtos = plans.Select(_mapper.Map<PlanDto>);
            return new GenericHttpResponse<IEnumerable<PlanDto>>(false, null, planDtos);
        }

        public async Task<GenericHttpResponse<PlanDto>> GetPlan(int planId)
        {
            var plan = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .FirstOrDefaultAsync(x => x.Id == planId);

            if (plan == null)
            {
                return new GenericHttpResponse<PlanDto>(true, new HttpError("Plan not found", HttpStatusCode.NotFound), null);
            }

            var planDto = _mapper.Map<PlanDto>(plan);
            return new GenericHttpResponse<PlanDto>(false, null, planDto);
        }

        public async Task<GenericHttpResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes()
        {
            var planTypes = await _unitOfWork.PlanTypes.All();

            if (planTypes == null)
            {
                return new GenericHttpResponse<IEnumerable<PlanTypeDto>>(true, new HttpError("Plan types not found", HttpStatusCode.NotFound), null);
            }

            var planTypeDtos = planTypes.Select(_mapper.Map<PlanTypeDto>);
            return new GenericHttpResponse<IEnumerable<PlanTypeDto>>(false, null, planTypeDtos);
        }


        public async Task<GenericHttpResponse<PlanDto>> CreatePlan(int packageId, CreatePlanDto createPlanDto)
        {
            var planType = await _unitOfWork.PlanTypes.GetById(createPlanDto.PlanTypeId);
            if (planType == null)
            {
                return new GenericHttpResponse<PlanDto>(true, new HttpError("Plan type not found", HttpStatusCode.NotFound), null);
            }

            var package = await _unitOfWork.Packages.GetById(packageId);
            if (package == null)
            {
                return new GenericHttpResponse<PlanDto>(true, new HttpError("Package not found", HttpStatusCode.NotFound), null);
            }

            var newPlan = new Plan()
            {
                Name = createPlanDto.Name,
                PlanType = planType,
                Package = package,
                Contribution = createPlanDto.Contribution,
            };

            await _unitOfWork.Plans.Add(newPlan);
            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<PlanDto>(false, null, _mapper.Map<PlanDto>(newPlan));
        }

        public async Task<GenericHttpResponse<PlanDto>> UpdatePlan(int planId, UpdatePlanDto updatePlanDto)
        {
            var plan = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .FirstOrDefaultAsync(x => x.Id == planId);
            if (plan == null)
            {
                return new GenericHttpResponse<PlanDto>(true, new HttpError("Plan not found", HttpStatusCode.NotFound), null);
            }

            var planType = await _unitOfWork.PlanTypes.GetById(updatePlanDto.PlanTypeId);
            if (planType == null)
            {
                return new GenericHttpResponse<PlanDto>(true, new HttpError("Plan type not found", HttpStatusCode.NotFound), null);
            }

            plan.Name = updatePlanDto.Name;
            plan.PlanType = planType;
            plan.Contribution = updatePlanDto.Contribution;

            _unitOfWork.Plans.Upsert(plan);
            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<PlanDto>(false, null, _mapper.Map<PlanDto>(plan));
        }
    }
}
