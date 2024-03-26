using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    public class EnrollmentsController : Controller
    {
        public IActionResult Index(int employerId)
        {
            return PartialView("Enrollments/_Enrollments");
        }
    }
}
