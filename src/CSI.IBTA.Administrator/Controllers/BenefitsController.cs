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
        public async Task<IActionResult> InsurancePackages(int employerId)
        {
            var res = await _benefitsServiceClient.GetInsurancePackages(employerId);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to retrieve employer's packages");
            }

            return PartialView("_EmployerPackagesMenu", (res.Result, employerId));
        }

        [HttpPatch("InitializePackage")]
        public async Task<IActionResult> InitializePackage(int employerId, int packageId)
        {
            var res = await _benefitsServiceClient.InitializeInsurancePackage(packageId);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to initialize insurance package");
            }

            return await InsurancePackages(employerId);
        }

        [HttpDelete("RemovePackage")]
        public async Task<IActionResult> RemovePackage(int employerId, int packageId)
        {
            var res = await _benefitsServiceClient.RemoveInsurancePackage(packageId);
            if (res.Error != null || res.Result == false)
            {
                throw new Exception("Failed to remove insurance package");
            }

            return await InsurancePackages(employerId);
        }
    }
}
