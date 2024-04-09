using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Customer.Filters;

namespace CSI.IBTA.Customer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("HomePartialView")]
        public IActionResult GetPartialView()
        {
            return PartialView("_Home");
        }
    }
}