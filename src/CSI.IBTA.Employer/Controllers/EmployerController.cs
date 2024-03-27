using CSI.IBTA.Emplopyer.Models;
using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("Employer")]
    public class EmployerController : Controller
    {
        private readonly IEmployersClient _employersClient;

        public EmployerController(IEmployersClient employersClient)
        {
            _employersClient = employersClient;
        }

        [HttpGet("{employerId}")]
        public async Task<IActionResult> Index(int employerId)
        {
            var res = await _employersClient.GetEmployerById(employerId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            var model = new EmployerProfileViewModel()
            {
                Employer = res.Result
            };

            return PartialView("EmployerProfile/_Profile", model);
        }

        [HttpGet("ProfileForm/{employerId}")]
        public async Task<IActionResult> GetProfileForm(int employerId)
        {
            var res = await _employersClient.GetEmployerById(employerId);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            var model = new EmployerProfileViewModel()
            {
                Employer = res.Result
            };

            return PartialView("EmployerProfile/_ProfileForm", model);
        }
    }
}
