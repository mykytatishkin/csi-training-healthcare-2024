using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {

        private readonly IEmployersClient _employersClient;

        public HomeController(IEmployersClient employersClient)
        {
            _employersClient = employersClient;
        }

        public async Task<IActionResult> Index()
        {
            return View("Index");
        }
    }
}
