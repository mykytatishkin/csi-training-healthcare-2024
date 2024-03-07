using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimsService;

        public ClaimsController(IClaimService claimsService)
        {
            _claimsService = claimsService;
        }

        [HttpGet("{claimId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetInsurancePackages(int claimId)
        {
            var response = await _claimsService.GetClaimDetails(claimId);

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
