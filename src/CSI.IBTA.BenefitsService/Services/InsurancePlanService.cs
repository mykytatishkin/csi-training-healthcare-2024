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

        public async Task<GenericResponse<IEnumerable<PlanDto>>> GetAllPlans(int? customerId = null)
        {

            var plans = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .Where(p => customerId == null || p.Enrollments.FirstOrDefault(e => e.EmployeeId == customerId) != null)
                .ToListAsync();

            var planDtos = plans.Select(_mapper.Map<PlanDto>);
            return new GenericResponse<IEnumerable<PlanDto>>(null, planDtos);
        }

        public async Task<GenericResponse<IEnumerable<PlanDto>>> GetActivePlansByNames(List<string> planNames, int employerId)
        {
            var distinctPlanNames = planNames.Distinct();

            var plans = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .Where(p => p.Package.EmployerId == employerId)
                .Where(p => planNames.Contains(p.Name))
                .Where(p => p.Package.PlanEnd > DateTime.UtcNow)
                .Where(p => p.Package.PlanStart <= DateTime.UtcNow)
                .Where(p => p.Package.Initialized.HasValue)
                .ToListAsync();

            var planDtos = plans.Select(_mapper.Map<PlanDto>);
            return new GenericResponse<IEnumerable<PlanDto>>(null, planDtos);
        }

        public async Task<GenericResponse<PlanDto>> GetPlan(int planId)
        {
            var plan = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .FirstOrDefaultAsync(x => x.Id == planId);

            if (plan == null)
            {
                return new GenericResponse<PlanDto>(new HttpError("Plan not found", HttpStatusCode.NotFound), null);
            }

            var planDto = _mapper.Map<PlanDto>(plan);
            return new GenericResponse<PlanDto>(null, planDto);
        }

        public async Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes()
        {
            var planTypes = await _unitOfWork.PlanTypes.All();

            if (planTypes == null)
            {
                return new GenericResponse<IEnumerable<PlanTypeDto>>(new HttpError("Plan types not found", HttpStatusCode.NotFound), null);
            }

            var planTypeDtos = planTypes.Select(_mapper.Map<PlanTypeDto>);
            return new GenericResponse<IEnumerable<PlanTypeDto>>(null, planTypeDtos);
        }

        public async Task<GenericResponse<PlanDto>> CreatePlan(int packageId, CreatePlanDto createPlanDto)
        {
            var planType = await _unitOfWork.PlanTypes.GetById(createPlanDto.PlanTypeId);
            if (planType == null)
            {
                return new GenericResponse<PlanDto>(new HttpError("Plan type not found", HttpStatusCode.NotFound), null);
            }

            var package = await _unitOfWork.Packages.GetById(packageId);
            if (package == null)
            {
                return new GenericResponse<PlanDto>(new HttpError("Package not found", HttpStatusCode.NotFound), null);
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
            return new GenericResponse<PlanDto>(null, _mapper.Map<PlanDto>(newPlan));
        }

        public async Task<GenericResponse<PlanDto>> UpdatePlan(int planId, UpdatePlanDto updatePlanDto)
        {
            var plan = await _unitOfWork.Plans
                .Include(x => x.Package)
                .Include(x => x.PlanType)
                .FirstOrDefaultAsync(x => x.Id == planId);
            if (plan == null)
            {
                return new GenericResponse<PlanDto>(new HttpError("Plan not found", HttpStatusCode.NotFound), null);
            }

            var planType = await _unitOfWork.PlanTypes.GetById(updatePlanDto.PlanType.Id);
            if (planType == null)
            {
                return new GenericResponse<PlanDto>(new HttpError("Plan type not found", HttpStatusCode.NotFound), null);
            }

            plan.Name = updatePlanDto.Name;
            plan.PlanType = planType;
            plan.Contribution = updatePlanDto.Contribution;

            _unitOfWork.Plans.Upsert(plan);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<PlanDto>(null, _mapper.Map<PlanDto>(plan));
        }
    }
}