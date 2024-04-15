using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.DB.Migrations.Migrations;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Extensions;
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
        public async Task<IActionResult> GetClaimsPaged(int page, int pageSize, string claimNumber = "", string employerId = "", string claimStatus = "")
        {
            var response = await _claimsService.GetClaims(page, pageSize, null, claimNumber, employerId, claimStatus);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("ByEmployee")]
        [Authorize(Roles = nameof(Role.Employee))]
        public async Task<IActionResult> GetEmployeeClaims(int page, int pageSize, int employeeId)
        {
            var userId = User.GetEmployeeId();

            if (userId == null || userId != employeeId)
            {
                return Problem(title: "UserId claim not found or invalid");
            }

            var response = await _claimsService.GetClaims(page, pageSize, employeeId: employeeId.ToString());

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

        [HttpPatch("{claimId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto)
        {
            var response = await _claimsService.UpdateClaim(claimId, updateClaimDto);
            
            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return NoContent();
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
