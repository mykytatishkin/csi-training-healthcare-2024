using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CSI.IBTA.Employer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class ContributionsController : Controller
    {
        private readonly IContributionService _contributionService;

        public ContributionsController(IContributionService contributionService)
        {
            _contributionService = contributionService;
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

            if (contributionsFileResponse.Result!.Errors.Sum(e => e.Value.Count) > 0)
            {
                // Return errors
            } else
            {
                // Send transactions
            }

            return Ok();
        }
    }
}
