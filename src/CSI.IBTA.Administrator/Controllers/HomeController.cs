using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserServiceClient _userServiceClient;

        public HomeController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<IActionResult> Index(string? nameFilter, string? codeFilter)
        {
            ViewData["CurrentNameFilter"] = nameFilter;
            ViewData["CurrentCodeFilter"] = codeFilter;

            var employers = await _userServiceClient.GetEmployers();
            if (employers != null) 
            {
                if (!String.IsNullOrEmpty(nameFilter))
                {
                    employers = employers.Where(s => s.Name.Contains(nameFilter)).ToList();
                }
                if (!String.IsNullOrEmpty(codeFilter))
                {
                    employers = employers.Where(s => s.Code.Contains(codeFilter)).ToList();
                }
            }
            
            return View(new HomeViewModel() { Employers = employers ?? new List<Employer>()});
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployer(CreateEmployerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = await _userServiceClient.CreateEmployer(model.ToDto());
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Description);
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
