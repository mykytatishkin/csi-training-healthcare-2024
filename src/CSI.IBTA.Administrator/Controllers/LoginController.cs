using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Constants;

namespace CSI.IBTA.Administrator.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthClient _client;

        public LoginController(IAuthClient client)
        {
            _client = client;
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

            var response = await _client.Authenticate(model.ToDto());
            if (!response.Success)
            {
                ModelState.AddModelError("", response.Description);
                return View("Index", model);
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
