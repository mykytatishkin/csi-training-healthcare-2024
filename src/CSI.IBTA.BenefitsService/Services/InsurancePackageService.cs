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

        public async Task<GenericResponse<CreatedInsurancePackageDto>> CreateInsurancePackage(CreateInsurancePackageDto dto)
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
            var packages = await _benefitsUnitOfWork.Packages.Find(x => x.EmployerId == employerId && x.IsRemoved != true);

            return new GenericResponse<List<InsurancePackageDto>>(null, packages.Select(_mapper.Map<InsurancePackageDto>).ToList());
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

                return new GenericResponse<FullInsurancePackageDto>(null, _mapper.Map<FullInsurancePackageDto>(package));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}