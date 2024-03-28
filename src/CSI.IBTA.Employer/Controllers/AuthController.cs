using CSI.IBTA.Employer.Models;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Extensions;
using CSI.IBTA.Employer.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace CSI.IBTA.Employer.Controllers
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

            return RedirectToAction("Index", "Home");
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
