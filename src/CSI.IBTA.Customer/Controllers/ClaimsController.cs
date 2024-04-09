using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
    [Route("[controller]")]
    public class ClaimsController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_Claims");
        }
    }
}
