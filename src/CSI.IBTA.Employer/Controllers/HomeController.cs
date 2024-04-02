using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
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
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var accountId = jwtSecurityToken.Subject;
            int intAccountId = Convert.ToInt32(accountId);

            var res = await _employersClient.GetEmployerByAccountId(intAccountId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            return View(res.Result);
        }
    }
}
