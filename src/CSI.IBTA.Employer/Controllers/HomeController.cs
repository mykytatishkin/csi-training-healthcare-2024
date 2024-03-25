using CSI.IBTA.Employer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSI.IBTA.Employer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
