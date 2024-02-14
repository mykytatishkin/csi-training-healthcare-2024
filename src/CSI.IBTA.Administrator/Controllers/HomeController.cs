using CSI.IBTA.Administrator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJwtTokenService _jwtTokenService;

        public HomeController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult Index()
        {
            var token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            bool isTokenValid = _jwtTokenService.IsTokenValid(token);

            if (!isTokenValid)
            {
                return RedirectToAction("Logout", "Auth");
            }

            return View();
        }
    }
}
