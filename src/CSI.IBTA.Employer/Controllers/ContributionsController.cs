using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Reflection;

namespace CSI.IBTA.Employer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("{controller}")]
    public class ContributionsController : Controller
    {
        private readonly IContributionsService _contributionService;
        private readonly IContributionsClient _contributionsClient;
        private readonly IJwtTokenService _jwtTokenService;

        public ContributionsController(IContributionsService contributionService, IContributionsClient contributionsClient, IJwtTokenService jwtTokenService)
        {
            _contributionService = contributionService;
            _contributionsClient = contributionsClient;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult Index()
        {
            return PartialView("_ImportContributions");
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return UnprocessableEntity();
            }

            var token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return Forbid();
            }

            var employerId = _jwtTokenService.GetEmployerId(token);

            if (employerId == null)
            {
                return Forbid();
            }
            
            var contributionsFileResponse = await _contributionService.ProcessContributionsFile(file, employerId.Value);

            if (contributionsFileResponse.Error != null)
            {
                return Problem(
                    title: contributionsFileResponse.Error.Title,
                    statusCode: (int)contributionsFileResponse.Error.StatusCode
                );
            }
            
            var processedResults = contributionsFileResponse.Result!;
            
            if (processedResults.Errors.Count > 0)
            {
                return BadRequest(new { processedResults.Errors });
            }
            else
            {
                var createContributionsResponse = await _contributionsClient.CreateContributions(processedResults.ProcessedContributions);

                if (createContributionsResponse.Error != null)
                {
                    return Problem(
                        title: createContributionsResponse.Error.Title,
                        statusCode: (int)createContributionsResponse.Error.StatusCode
                    );
                }
            }

            return Ok();
        }
    }
}
