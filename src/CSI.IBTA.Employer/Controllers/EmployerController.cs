using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    public class EmployerController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_Employer");
        }
    }
}
