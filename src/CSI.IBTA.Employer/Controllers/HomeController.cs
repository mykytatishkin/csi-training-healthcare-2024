using CSI.IBTA.Employer.Extensions;
using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CSI.IBTA.Employer.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        private readonly IEmployersClient _employersClient;
        private readonly IJwtTokenService _jwtTokenService;

        public HomeController(IEmployersClient employersClient, IJwtTokenService jwtTokenService)
        {
            _employersClient = employersClient;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<IActionResult> Index()
        {
            var token = _jwtTokenService.GetCachedToken();
            var employerId = JwtSecurityTokenExtensions.GetEmployerId(token);

            if (employerId == null)
            {
                return Problem(title: "Employer ID claim not found or invalid");
            }

            var res = await _employersClient.GetEmployerById((int) employerId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            return View(res.Result);
        }
    }
}
