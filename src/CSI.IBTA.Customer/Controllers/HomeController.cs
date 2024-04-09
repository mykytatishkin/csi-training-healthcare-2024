using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
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
