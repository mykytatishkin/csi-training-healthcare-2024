using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_Settings");
        }
    }
}
