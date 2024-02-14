using CSI.IBTA.Administrator.Constants;
using CSI.IBTA.Administrator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenService _jwtTokenService;

        public HomeController(IHttpContextAccessor httpContextAccessor, IJwtTokenService jwtTokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult Index()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string? token = httpContext.Request.Cookies[TokenConstants.JwtTokenCookieName];

            if (token == null)
            {
                return RedirectToAction("Login", "Login");
            }

            bool isValid = _jwtTokenService.IsTokenValid(token);

            if (!isValid)
            {
                return RedirectToAction("Logout", "Login");
            }

            return View();
        }
    }
}
