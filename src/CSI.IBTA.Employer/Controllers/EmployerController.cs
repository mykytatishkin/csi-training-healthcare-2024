using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Employer.Models;
using CSI.IBTA.Employer.Filters;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("Employer")]
    [TypeFilter(typeof(AuthenticationFilter))]
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

        [HttpPut]
        public async Task<IActionResult> UpdateEmployer(EmployerProfileViewModel model)
        {
            var e = model.Employer;
            var res = await _employersClient.UpdateEmployer(new UpdateEmployerDto(e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.ZipCode, e.Phone, model.NewLogo), e.Id);
            return Json(res);
        }
    }
}
