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

        [HttpGet("CreateEmployerForm")]
        public IActionResult CreateEmployerForm()
        {
            return PartialView("_EmployerForm", new EmployerFormViewModel());
        }

        [HttpGet("UpdateEmployerForm")]
        public async Task<IActionResult> UpdateEmployerForm(int employerId)
        {
            var response = await _userServiceClient.GetEmployerById(employerId);

            if (response.Error != null || response.Result == null)
            {
                throw new Exception("Failed to retrieve employer");
            }

            return PartialView("_EmployerForm", response.Result.ToFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployer(EmployerFormViewModel model)
        {
            var res = await _userServiceClient.CreateEmployer(model.ToCreateEmployerDto());
            return Json(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployer(EmployerFormViewModel model)
        {
            var res = await _userServiceClient.UpdateEmployer(model.ToUpdateEmployerDto(), model.Id ?? 0);
            return Json(res);
        }
    }
}
