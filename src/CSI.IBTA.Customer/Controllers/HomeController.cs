using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Customer.Filters;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Extensions;

namespace CSI.IBTA.Customer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        private readonly IEmployeesClient _employeesClient;
        private readonly IJwtTokenService _jwtTokenService;

        public HomeController(IEmployeesClient employesClient, IJwtTokenService jwtTokenService)
        {
            _employeesClient = employesClient;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<IActionResult> Index()
        {
            var token = _jwtTokenService.GetCachedToken();
            var employeeId = JwtSecurityTokenExtensions.GetEmployeeId(token);
            
            if (employeeId == null)
            {
                return Problem(title: "Employee ID claim not found or invalid");
            }

            var res = await _employeesClient.GetEmployee((int)employeeId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employee");
            }

            return View(res.Result);
        }

        [HttpGet("HomePartialView")]
        public async Task<IActionResult> GetPartialView()
        {
            var token = _jwtTokenService.GetCachedToken();
            var employeeId = JwtSecurityTokenExtensions.GetEmployeeId(token);

            if (employeeId == null)
            {
                return Problem(title: "Employee ID claim not found or invalid");
            }

            var res = await _employeesClient.GetEmployee((int) employeeId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employee");
            }

            return PartialView("_Home", res.Result);
        }

        [HttpGet("EmployerLogo")]
        public async Task<string> GetEmployerLogo()
        {
            var res = await _employeesClient.GetEmployerLogo();
            return res.Result ?? "";
        }
    }
}