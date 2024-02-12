using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Endpoints;

namespace CSI.IBTA.Administrator.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthClient _client;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginController(IAuthClient client, IJwtTokenService jwtTokenService)
        {
            _client = client;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var response = await _client.PostAsync(model.ToDto(), AuthApiEndpoints.Auth);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    ModelState.AddModelError("", "Server error");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials");
                }
                return View("Index", model);
            }

            var (isAdmin, token) = await _jwtTokenService.CheckUserIsAdminAsync(response);

            if(!isAdmin)
            {
                ModelState.AddModelError("", "Access to administrator portal denied");
                return View("Index", model);
            }

            var cookieOptions =_jwtTokenService.GetCookieOptions();
            Response.Cookies.Append("jwtToken", token, cookieOptions);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
