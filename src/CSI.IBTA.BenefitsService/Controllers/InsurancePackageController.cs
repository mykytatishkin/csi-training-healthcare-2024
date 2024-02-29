using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InsurancePackageController : Controller
    {
        [HttpPost]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> CreateEmployer(CreateInsurancePackageDto dto)
        {
            var response = await _employerService.CreateEmployer(dto);

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