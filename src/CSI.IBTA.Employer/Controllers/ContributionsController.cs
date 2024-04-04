using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CSI.IBTA.Employer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("{controller}")]
    public class ContributionsController : Controller
    {
        private readonly IContributionsService _contributionService;
        private readonly IContributionsClient _contributionsClient;

        public ContributionsController(IContributionsService contributionService, IContributionsClient contributionsClient)
        {
            _contributionService = contributionService;
            _contributionsClient = contributionsClient;
        }

        public IActionResult Index()
        {
            return PartialView("_ImportContributions");
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Fix
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return View(file);
            }

            var contributionsFileResponse = await _contributionService.ProcessContributionsFile(file);

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
