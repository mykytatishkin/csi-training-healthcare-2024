using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Employer")]
    public class EmployerController : Controller
    {
       
        private readonly IUserServiceClient _userServiceClient;

        public EmployerController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<IActionResult> Index(int employerId)
        {
            var response = await _userServiceClient.GetEmployerById(employerId);

            if (response.Error != null)
            {
                throw new Exception("Failed to retrieve employer");
            }

            return PartialView("_EmployerAdministration", response.Result);
        }

        [HttpGet("AdministrationMenu")]
        public IActionResult AdministrationMenu(int employerId)
        {
            return PartialView("_EmployerAdministrationMenu", employerId);
        }

        [HttpGet("EmployerForm")]
        public async Task<IActionResult> EmployerForm()
        {
            return PartialView("_EmployerForm");
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployer(CreateEmployerViewModel model)
        {
            var res = await _userServiceClient.CreateEmployer(model.ToDto());
            return Json(res);
        }
    }
}
