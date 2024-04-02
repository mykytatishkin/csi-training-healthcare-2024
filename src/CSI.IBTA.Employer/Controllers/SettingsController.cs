using CSI.IBTA.Employer.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_Settings");
        }
    }
}
