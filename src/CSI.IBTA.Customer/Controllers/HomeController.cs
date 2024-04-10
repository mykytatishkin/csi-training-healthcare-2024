using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Customer.Filters;

namespace CSI.IBTA.Customer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("HomePartialView/{emploeeId}")]
        public IActionResult GetPartialView(int employeeId)
        {
            var employee = new FullEmployeeDto(employeeId, "Employee", "", "John", "Smith", "15547A", "4544485747", new DateOnly(2000, 10, 9), "johnsmith@gmail.com", "Kauno", "Studentu st.", "Kaunas", "525861", 4);
            return PartialView("_Home", employee);
        }
    }
}