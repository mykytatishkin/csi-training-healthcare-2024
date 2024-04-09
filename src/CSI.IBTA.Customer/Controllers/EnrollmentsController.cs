using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
    [Route("[controller]")]
    public class EnrollmentsController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_Enrollments");
        }
    }
}
