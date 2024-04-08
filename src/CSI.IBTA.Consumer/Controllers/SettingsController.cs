using CSI.IBTA.Consumer.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Consumer.Controllers
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
