using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;

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

        [HttpPost]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> CreateInsurancePackage(CreateInsurancePackageDto dto)
        {
            var response = await _insurancePackageService.CreateInsurancePackage(dto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
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