using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("InsurancePackage")]
    public class InsurancePackageController : Controller
    {
        private readonly IInsurancePackageClient _packageClient;

        public InsurancePackageController(IInsurancePackageClient packageClient)
        {
            _packageClient = packageClient;
        }

        public async Task<IActionResult> Index(int employerId)
        {
            var getPlanTypesResponse = await _packageClient.GetPlanTypes();
            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }
            var PlanTypes = getPlanTypesResponse.Result;

            var viewModel = new InsurancePackageCreationViewModel
            {
                EmployerId = employerId,
                AvailablePlanTypes = PlanTypes.Select(x => new PlanType()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };

            return PartialView("InsurancePackages/_CreateInsurancePackage", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInsurancePackage(InsurancePackageCreationViewModel viewModel)
        {
            List<CreatePlanDto> planDtos = [];

            if (viewModel.Plans != null)
            {
                planDtos = viewModel.Plans
                    .Select(p => new CreatePlanDto(
                        p.Name,
                        p.Contribution,
                        p.TypeId))
                    .ToList();
            }

            var command = new CreateInsurancePackageDto(
                viewModel.Package.Name,
                viewModel.Package.PlanStart,
                viewModel.Package.PlanEnd,
                viewModel.Package.PayrollFrequency,
                viewModel.EmployerId,
                planDtos);

            var response = await _packageClient.CreateInsurancePackage(command);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok();
        }

        [HttpGet("InsurancePackages")]
        public async Task<IActionResult> InsurancePackages(int employerId)
        {
            var res = await _packageClient.GetInsurancePackages(employerId);

            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to retrieve employer's packages");
            }

            var viewModel = new InsurancePackageViewModel
            {
                InsurancePackages = res.Result,
                EmployerId = employerId
            };

            return PartialView("_EmployerPackagesMenu", viewModel);
        }

        [HttpPatch("InitializePackage")]
        public async Task<IActionResult> InitializePackage(int employerId, int packageId)
        {
            var res = await _packageClient.InitializeInsurancePackage(packageId);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to initialize insurance package");
            }

            return await InsurancePackages(employerId);
        }

        [HttpDelete("RemovePackage")]
        public async Task<IActionResult> RemovePackage(int employerId, int packageId)
        {
            var res = await _packageClient.RemoveInsurancePackage(packageId);
            if (res.Error != null || res.Result == false)
            {
                throw new Exception("Failed to remove insurance package");
            }

            return await InsurancePackages(employerId);
        }
    }
}
