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

        [HttpGet("EmployerForm")]
        public async Task<IActionResult> EmployerForm()
        {
            return PartialView("_EmployerForm");
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployer(CreateEmployerViewModel model)
        {
            var res = await _userServiceClient.CreateEmployer(model.ToDto());
            if (!res.Success)
            {
                return Json(new { success = false, errorMessage = res.Description });
            }

            return Json(new { success = true });
        }
    }
}
