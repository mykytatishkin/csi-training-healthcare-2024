using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClaimsController : Controller
    {
        private readonly IClaimsService _claimsService;

        public ClaimsController(IClaimsService claimsService)
        {
            _claimsService = claimsService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetClaims()
        {
            var response = await _claimsService.GetClaims();

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("{claimId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetClaim(int claimId)
        {
            var response = await _claimsService.GetClaim(claimId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPatch("Approve/{claimId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var response = await _claimsService.ApproveClaim(claimId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return NoContent();
        }

        [HttpPatch("Deny/{claimId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> DenyClaim(int claimId, DenyClaimDto dto)
        {
            var response = await _claimsService.DenyClaim(claimId, dto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return NoContent();
        }
    }
}
