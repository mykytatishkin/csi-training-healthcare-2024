using CSI.IBTA.Employer.Models;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Extensions;
using CSI.IBTA.Employer.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace CSI.IBTA.Administrator.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthClient _client;
        private readonly IEmployersClient _employersClient;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IAuthClient client, IEmployersClient employersClient, IJwtTokenService jwtTokenService)
        {
            _client = client;
            _employersClient = employersClient;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var response = await _client.Authenticate(model.ToDto());
            if (!response.Result)
            {
                ModelState.AddModelError("", response.Error.Title);
                return View("Index");
            }

            var token = _jwtTokenService.GetCachedToken();

            if (_jwtTokenService.IsTokenValid(token))
            {
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var employerIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "employer_id");
                if (employerIdClaim != null && int.TryParse(employerIdClaim.Value, out int employerId))
                {
                    var employerInfo = await _employersClient.GetEmployerById(employerId);

                    if (employerInfo.Result != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Failed to retrieve employer information");
            return View("Index");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete(TokenConstants.JwtTokenCookieName);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
