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
        private readonly IPlansClient _plansClient;

        public InsurancePackageController(IInsurancePackageClient packageClient, IPlansClient plansClient)
        {
            _packageClient = packageClient;
            _plansClient = plansClient;
        }

        public async Task<IActionResult> Index(int employerId)
        {
            var getPlanTypesResponse = await _plansClient.GetPlanTypes();

            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }

            var planTypes = getPlanTypesResponse.Result;

            var viewModel = new InsurancePackageFormViewModel
            {
                Package = new FullInsurancePackageDto(0, "", DateTime.UtcNow, DateTime.UtcNow, PayrollFrequency.Weekly, 0, new List<PlanDto>()),
                EmployerId = employerId,
                AvailablePlanTypes = planTypes,
            };

            return PartialView("InsurancePackages/_PackageForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInsurancePackage(InsurancePackageFormViewModel viewModel)
        {
            var dto = new CreateInsurancePackageDto(
                viewModel.Package.Name,
                viewModel.Package.PlanStart,
                viewModel.Package.PlanEnd,
                viewModel.Package.PayrollFrequency,
                viewModel.EmployerId,
                viewModel.Package.Plans
                    .Select(p => new CreatePlanDto(
                        p.Name,
                        p.Contribution,
                        p.PlanType.Id))
                    .ToList());

            var response = await _packageClient.CreateInsurancePackage(dto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok();
        }

        [HttpGet("UpdateInsurancePackage")]
        public async Task<IActionResult> UpdateInsurancePackage(int employerId, int insurancePackageId)
        {
            var packageDetails = await _packageClient.GetInsurancePackage(insurancePackageId);

            if (packageDetails.Error != null)
            {
                return Problem(
                    statusCode: (int)packageDetails.Error.StatusCode,
                    title: packageDetails.Error.Title);
            }

            var planTypes = await _plansClient.GetPlanTypes();
            if (planTypes.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }

            var viewModel = new InsurancePackageFormViewModel
            {
                EmployerId = employerId,
                Package = packageDetails.Result,
                AvailablePlanTypes = planTypes.Result
            };

            return PartialView("InsurancePackages/_PackageForm", viewModel);
        }

        [HttpPut("UpsertInsurancePackage")]
        public async Task<IActionResult> UpsertInsurancePackage(InsurancePackageFormViewModel viewModel)
        {
            if (viewModel.Package.Id == 0) return await CreateInsurancePackage(viewModel);

            var dto = new UpdateInsurancePackageDto(viewModel.Package.Id, viewModel.Package.Name, viewModel.Package.PlanStart, viewModel.Package.PlanEnd, viewModel.Package.PayrollFrequency, viewModel.EmployerId, viewModel.Package.Plans);
            var response = await _packageClient.UpdateInsurancePackage(dto);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error.Title);
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