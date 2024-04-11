using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Customer.Filters;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Extensions;

namespace CSI.IBTA.Customer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmployeesClient _employeesClient;

        public HomeController(IJwtTokenService jwtTokenService, IEmployeesClient employeesClient)
        {
            _jwtTokenService = jwtTokenService;
            _employeesClient = employeesClient;
        }

        public async Task<IActionResult> Index()
        {
            var token = _jwtTokenService.GetCachedToken();
            var userId = JwtSecurityTokenExtensions.GetUserId(token);

            if (userId == null)
            {
                return Problem(title: "User ID claim not found or invalid");
            }

            var res = await _employeesClient.GetEmployee((int)userId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employee");
            }

            return View(res.Result);
        }

        [HttpGet("HomePartialView")]
        public IActionResult GetPartialView()
        {
            return PartialView("_Home");
        }
    }
}