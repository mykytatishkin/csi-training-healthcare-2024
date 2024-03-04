using CSI.IBTA.Administrator.Interfaces;
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

            return PartialView("_EmployerPackagesMenu", res.Result);
        }
    }
}
