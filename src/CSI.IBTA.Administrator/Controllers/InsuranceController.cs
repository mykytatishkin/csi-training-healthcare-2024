using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class InsuranceController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("InsurancePackages/_CreateInsurancePackage");
        }
    }
}
