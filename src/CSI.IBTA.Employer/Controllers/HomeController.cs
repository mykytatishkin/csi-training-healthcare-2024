using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
