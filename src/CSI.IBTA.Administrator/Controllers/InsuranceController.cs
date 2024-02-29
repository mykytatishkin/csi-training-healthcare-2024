using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Insurance")]
    public class InsuranceController : Controller
    {
        private readonly IInsurancePackageClient _packageClient;
        
        public InsuranceController(IInsurancePackageClient packageClient)
        {
            _packageClient = packageClient;
        }

        public IActionResult Index()
        {
            return PartialView("InsurancePackages/_CreateInsurancePackage");
        }

        [HttpPost("CreateInsurancePackage")]
        public async Task<IActionResult> CreateInsurancePackage(InsurancePackageCreationViewModel viewModel)
        {
            var command = new CreateInsurancePackageDto(
                viewModel.Package.Name,
                viewModel.Package.PlanStart,
                viewModel.Package.PlanEnd,
                viewModel.Package.PayrollFrequency,
                viewModel.Plans ?? []);

            var response = await _packageClient.CreateInsurancePackage(command);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return PartialView("InsurancePackages/_CreateInsurancePackage");
        }
    }
}
