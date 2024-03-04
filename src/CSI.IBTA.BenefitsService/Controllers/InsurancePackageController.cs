using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InsurancePackageController : Controller
    {
        private readonly IInsurancePackageService _insurancePackageService;

        public InsurancePackageController(IInsurancePackageService insurancePackageService)
        {
            _insurancePackageService = insurancePackageService;
        }

        [HttpGet("{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetInsurancePackages(int employerId)
        {
            var response = await _insurancePackageService.GetInsurancePackages(employerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }
    }
}