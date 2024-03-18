using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using System.Net;
using AutoMapper;
using CSI.IBTA.DB.Migrations.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CSI.IBTA.DataLayer.Extentions;

using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.BenefitsService.Services
{
    public class InsurancePackageService : IInsurancePackageService
    {
        private readonly IBenefitsUnitOfWork _benefitsUnitOfWork;
        private readonly IMapper _mapper;

        public InsurancePackageService(IBenefitsUnitOfWork benefitsUnitOfWork, IMapper mapper)
        {
            _benefitsUnitOfWork = benefitsUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<CreatedInsurancePackageDto>> CreateInsurancePackage(
            CreateInsurancePackageDto dto)
        {
            var existingPackage = await _benefitsUnitOfWork.Packages.Find(p => p.Name == dto.Name);

            if (existingPackage.Any())
            {
                var error = new HttpError("Insurance package with this name already exists", HttpStatusCode.Conflict);
                return new(error, null);
            }

            bool samePlanNames = dto.Plans
                .Select(p => p.Name)
                .Distinct()
                .Count() < dto.Plans.Count;

            if (samePlanNames)
            {
                var error = new HttpError("Multiple plans cannot have same name", HttpStatusCode.Conflict);
                return new(error, null);
            }

            var package = new Package
            {
                EmployerId = dto.EmployerId,
                PlanEnd = dto.PlanEnd,
                Name = dto.Name,
                PayrollFrequency = dto.PayrollFrequency,
                PlanStart = dto.PlanStart
            };

            await _benefitsUnitOfWork.Packages.Add(package);

            var plans = new List<Plan>();

            foreach (CreatePlanDto planDto in dto.Plans)
            {
                var plan = new Plan
                {
                    Package = package,
                    Contribution = planDto.Contribution,
                    Name = planDto.Name,
                    TypeId = planDto.PlanTypeId
                };

                plans.Add(plan);
                await _benefitsUnitOfWork.Plans.Add(plan);
            }

            await _benefitsUnitOfWork.CompleteAsync();

            var createdPlans = plans
                .Select(p => new CreatedPlanDto(
                    p.Id, p.Name, p.TypeId, p.Contribution))
                .ToList();

            var createdPackage = new CreatedInsurancePackageDto(
                package.Id,
                package.Name,
                package.PlanStart,
                package.PlanEnd,
                package.PayrollFrequency,
                package.EmployerId,
                createdPlans);

            return new(null, createdPackage);
        }

        public async Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var packages =
                await _benefitsUnitOfWork.Packages.Find(x => x.EmployerId == employerId && x.IsRemoved != true);

            return new GenericResponse<List<InsurancePackageDto>>(null,
                packages.Select(_mapper.Map<InsurancePackageDto>).ToList());
        }

        public async Task<GenericResponse<FullInsurancePackageDto>> GetInsurancePackage(int packageId)
        {
            try
            {
                var plans = await _benefitsUnitOfWork.Plans.Include(x => x.PlanType)
                    .Where(x => x.PackageId == packageId).ToListAsync();

                var package = await _benefitsUnitOfWork.Packages.GetById(packageId);

                package.Plans = plans;

                if (plans == null)
                {
                    var error = new HttpError("Insurance package not found", HttpStatusCode.NotFound);
                    return new GenericResponse<FullInsurancePackageDto>(error, null);
                }

                return new GenericResponse<FullInsurancePackageDto>(null,
                    _mapper.Map<FullInsurancePackageDto>(package));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GenericResponse<FullInsurancePackageDto>> UpdateInsurancePackage(UpdateInsurancePackageDto dto,
            int packageId)
        {
            var existingPackage = await _benefitsUnitOfWork.Packages.GetById(packageId);
            if (existingPackage == null)
            {
                var error = new HttpError("Package not found", HttpStatusCode.NotFound);
                return new(error, null);
            }

            var existingPackageWithSameName = await _benefitsUnitOfWork.Packages
                .Find(p => p.Name == dto.Name && p.Id != packageId);
            if (existingPackageWithSameName.Any())
            {
                var error = new HttpError("Other insurance package with this name already exists", HttpStatusCode.Conflict);
                return new GenericResponse<FullInsurancePackageDto>(error, null);
            }

            var existingPlans = await _benefitsUnitOfWork.Plans.Find(x => x.PackageId == packageId);
            existingPackage.Plans = (IList<Plan>)existingPlans;

            bool samePlanNames = dto.Plans
                .Select(p => p.Name)
                .Distinct()
                .Count() < dto.Plans.Count;
            if (samePlanNames)
            {
                var error = new HttpError("Multiple plans cannot have same name", HttpStatusCode.Conflict);
                return new(error, null);
            }

            existingPackage.EmployerId = dto.EmployerId;
            existingPackage.PlanEnd = dto.PlanEnd;
            existingPackage.Name = dto.Name;
            existingPackage.PayrollFrequency = dto.PayrollFrequency;
            existingPackage.PlanStart = dto.PlanStart;

            foreach (var planDto in dto.Plans)
            {
                var existingPlan = existingPackage.Plans.FirstOrDefault(p => p.Name == planDto.Name);
                if (existingPlan != null)
                {
                    existingPlan.Contribution = planDto.Contribution;
                    existingPlan.TypeId = planDto.PlanTypeId;
                    existingPlan.Name = planDto.Name;
                    existingPlan.PlanType = await _benefitsUnitOfWork.PlanTypes.GetById(planDto.PlanTypeId);
                }
                else
                {
                    var newPlan = new Plan
                    {
                        PackageId = existingPackage.Id,
                        Contribution = planDto.Contribution,
                        Name = planDto.Name,
                        TypeId = planDto.PlanTypeId,
                        Package = existingPackage,
                        PlanType = await _benefitsUnitOfWork.PlanTypes.GetById(planDto.PlanTypeId)
                    };

                    existingPackage.Plans.Add(newPlan);
                }
            }

            _benefitsUnitOfWork.Packages.Upsert(existingPackage);

            try
            {
                await _benefitsUnitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var createdPlans = existingPackage.Plans
                .Select(p => new CreatedPlanDto(p.Id, p.Name, p.TypeId, p.Contribution))
                .ToList();

            var createdPackage = new FullInsurancePackageDto(
                existingPackage.Id,
                existingPackage.Name,
                existingPackage.PlanStart,
                existingPackage.PlanEnd,
                existingPackage.PayrollFrequency,
                existingPackage.EmployerId,
                createdPlans.ConvertAll(x => new PlanDto(x.Id, x.Name, new PlanTypeDto(x.PlanTypeId, ""), x.Contribution, x.Id)));

            return new(null, createdPackage);
        }

        public async Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId)
        {
            var package = await _benefitsUnitOfWork.Packages.GetById(packageId);
            if (package == null) return new GenericResponse<InsurancePackageDto>(HttpErrors.ResourceNotFound, null);

            package.Initialized = DateOnly.FromDateTime(DateTime.UtcNow);
            _benefitsUnitOfWork.Packages.Upsert(package);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<InsurancePackageDto>(null, _mapper.Map<InsurancePackageDto>(package));
        }

        public async Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId)
        {
            var package = await _benefitsUnitOfWork.Packages.GetById(packageId);
            if (package == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            package.IsRemoved = true;
            _benefitsUnitOfWork.Packages.Upsert(package);
            await _benefitsUnitOfWork.CompleteAsync();

            return new GenericResponse<bool>(null, true);
        }
    }
}