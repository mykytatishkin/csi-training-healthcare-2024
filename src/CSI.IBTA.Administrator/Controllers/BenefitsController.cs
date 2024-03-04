using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Benefits")]
    public class BenefitsController : Controller
    {
       
        private readonly IUserServiceClient _userServiceClient;

        public BenefitsController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        [HttpGet("InsurancePackages")]
        public IActionResult AdministrationMenu(int employerId)
        {
            var list = new List<InsurancePackageDto>()
            {
                new InsurancePackageDto(2,"ISSoft Package", "Not Initialized", true, false),
                new InsurancePackageDto(3," Package", $"Initialized on {employerId}", false, true)
            };
            return PartialView("_EmployerPackagesMenu", list);
        }
    }
}
