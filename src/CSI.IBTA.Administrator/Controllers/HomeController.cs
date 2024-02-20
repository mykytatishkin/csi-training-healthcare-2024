using CSI.IBTA.Administrator.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly IJwtTokenService _jwtTokenService;

        public HomeController(IJwtTokenService jwtTokenService, IUserServiceClient userServiceClient)
        {
            _jwtTokenService = jwtTokenService;
            _userServiceClient = userServiceClient;
        }

        public async Task<IActionResult> Index(string nameFilter, string codeFilter)
        {
            ViewData["CurrentNameFilter"] = nameFilter;
            ViewData["CurrentCodeFilter"] = codeFilter;

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

            var Employers = await _userServiceClient.GetEmployers(token);
            if (Employers != null) 
            {
                if (!String.IsNullOrEmpty(nameFilter))
                {
                    Employers = Employers.Where(s => s.Name.Contains(nameFilter)).ToList();
                }
                if (!String.IsNullOrEmpty(codeFilter))
                {
                    Employers = Employers.Where(s => s.Code.Contains(codeFilter)).ToList();
                }
            }


            return View(Employers);
        }

    }
}
