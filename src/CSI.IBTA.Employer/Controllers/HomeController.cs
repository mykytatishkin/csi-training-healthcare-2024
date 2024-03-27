using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    public class HomeController : Controller
    {

        private readonly IEmployersClient _employersClient;

        public HomeController(IEmployersClient employersClient)
        {
            _employersClient = employersClient;
        }

        public async Task<IActionResult> Index()
        {
            //Hardcoded employer id until authentication is implemented 
            var res = await _employersClient.GetEmployerById(5);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            return View(res.Result);
        }
    }
}
