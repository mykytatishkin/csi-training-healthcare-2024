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

        [HttpGet("UpdateInsurancePackage")]
        public async Task<IActionResult> UpdateInsurancePackage(int employerId, int insurancePackageId)
        {
            var packageDetails = await _packageClient.GetInsurancePackage(insurancePackageId);

            if (packageDetails.Error != null)
            {
                return Problem(
                    title: packageDetails.Error.Title,
                    statusCode: (int)packageDetails.Error.StatusCode
                );
            }

            var getPlanTypesResponse = await _packageClient.GetPlanTypes();
            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }
            var PlanTypes = getPlanTypesResponse.Result;

            var viewModel = new InsurancePackageModificationViewModel
            {
                EmployerId = employerId,
                Package = packageDetails.Result,
                Plans = packageDetails.Result.Plans,
                AvailablePlanTypes = PlanTypes.Select(x => new PlanTypeDto(x.Id, x.Name)).ToList(),
            };

            return PartialView("InsurancePackages/_ModifyInsurancePackage", viewModel);
        }

        [HttpPost("UpdateInsurancePackage")]
        public async Task<IActionResult> UpdateInsurancePackage(InsurancePackageModificationViewModel viewModel)
        {
            List<UpdatePlanDto> planDtos = [];

            if (viewModel.Plans != null)
            {
                planDtos = viewModel.Plans
                    .Select(p => new UpdatePlanDto(
                        p.Name,
                        p.Contribution,
                        p.TypeId))
                    .ToList();
            }

            var command = new FullInsurancePackageDto(
                viewModel.Package.Name,
                viewModel.Package.PlanStart,
                viewModel.Package.PlanEnd,
                viewModel.Package.PayrollFrequency,
                viewModel.EmployerId,
                planDtos);

            var response = await _packageClient.UpdateInsurancePackage(command);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok();
        }
    }
}