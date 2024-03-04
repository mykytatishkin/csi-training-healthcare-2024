using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
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

        public IActionResult Index(int employerId)
        {
            var viewModel = new InsurancePackageCreationViewModel
            {
                EmployerId = employerId
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
    }
}
