using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserClient _userClient;

        public HomeController(IJwtTokenService jwtTokenService, IUserClient userClient)
        {
            _jwtTokenService = jwtTokenService;
            _userClient = userClient;
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

        [HttpPost]
        public async Task<IActionResult> AddEmployer(CreateEmployerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = await _userClient.CreateEmployer(model.ToDto());
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Description);
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
