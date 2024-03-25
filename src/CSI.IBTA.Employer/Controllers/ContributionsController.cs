using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    public class ContributionsController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_ImportContributions");
        }
    }
}
