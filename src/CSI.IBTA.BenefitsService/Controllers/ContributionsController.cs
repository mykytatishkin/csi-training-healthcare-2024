using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContributionsController : Controller
    {
        [HttpPost]
        public IActionResult Post(List<ProcessedContributionDto> contributionEntries)
        {
            Console.WriteLine("Contribution count: " + contributionEntries.Count);
            return Ok();
        }
    }
}
