using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class EmployerUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ModelState.AddModelError("", "Test error");
            return View("Index");

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
