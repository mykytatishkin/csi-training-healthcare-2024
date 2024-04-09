using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Extensions;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContributionsController : Controller
    {
        private readonly IContributionsService _contributionsService;

        public ContributionsController(IContributionsService contributionsService)
        {
            _contributionsService = contributionsService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.EmployerAdmin))]
        public async Task<IActionResult> Post(List<ProcessedContributionDto> contributionEntries)
        {
            var signedEmployerId = User.GetEmployerId();

            if (signedEmployerId == null)
            {
                return Problem(title: "EmployerId claim not found or invalid");
            }

            var response = await _contributionsService.CreateContributions(contributionEntries, signedEmployerId.Value);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok();
        }
    }
}
