using CSI.IBTA.Consumer.Filters;
using CSI.IBTA.Consumer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Consumer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
