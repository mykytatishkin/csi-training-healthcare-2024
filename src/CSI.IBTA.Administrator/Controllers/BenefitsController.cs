using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Benefits")]
    public class BenefitsController : Controller
    {
       
        private readonly IBenefitsServiceClient _benefitsServiceClient;

        public BenefitsController(IBenefitsServiceClient benefitsServiceClient)
        {
            _benefitsServiceClient = benefitsServiceClient;
        }

        [HttpGet("InsurancePackages")]
        public async Task<IActionResult> AdministrationMenu(int employerId)
        {
            var res = await _benefitsServiceClient.GetInsurancePackages(employerId);

            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to retrieve employer");
            }

            var viewModel = new InsurancePackageViewModel
            {
                InsurancePackages = res.Result,
                EmployerId = employerId
            };

            return PartialView("_EmployerPackagesMenu", viewModel);
        }
    }
}
