using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("{controller}")]
    public class EmployeesController : Controller
    {
        [HttpGet]
        public IActionResult Index(int employerId)
        {
            return PartialView("_Employees");
        }
    }
}
