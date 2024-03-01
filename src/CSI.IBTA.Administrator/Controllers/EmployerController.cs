using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Administrator.Extensions;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("Employer")]
    public class EmployerController : Controller
    {
        private readonly IEmployerClient _employerClient;
        private readonly IEmployerUserClient _employerUserClient;
        private readonly IUserServiceClient _userServiceClient;

        public EmployerController(
            IEmployerClient employerClient,
            IEmployerUserClient employerUserClient,
            IUserServiceClient userServiceClient)
        {
            _employerClient = employerClient;
            _employerUserClient = employerUserClient;
            _userServiceClient = userServiceClient;
        }

        public async Task<IActionResult> Index(int employerId)
        {
            var response = await _employerClient.GetEmployerById(employerId);

            if (response.Error != null)
            {
                return Problem(title: "Failed to retrieve employer");
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
        public async Task<ActionResult> UpdateEmployerForm(int employerId)
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
        [HttpGet("Users")]
        public async Task<IActionResult> Users(int employerId)
        {
            var response = await _employerUserClient.GetEmployerUsers(employerId);

            if (response.Error != null || response.Result == null)
            {
                return Problem(title: "Failed to retrieve employer users");
            }

            var viewModel = new UserManagementViewModel
            {
                EmployerId = employerId,
                EmployerUsers = response.Result
            };

            return PartialView("_EmployerAdministrationUserManagement", viewModel);
        }

        [HttpGet("CreateUser")]
        public IActionResult CreateUser(int employerId)
        {
            var viewModel = new EmployerUserViewModel
            {
                ActionName = "CreateUser",
                EmployerId = employerId
            };

            return PartialView("_EmployerCreateUserSection", viewModel);
        }

        [HttpGet("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int userId, int employerId)
        {
            var response = await _userServiceClient.GetUser(userId);

            if (response.Error != null || response.Result == null)
            {
                return Problem(title: "Failed to retrieve user");
            }

            var viewModel = new EmployerUserViewModel
            {
                ActionName = "UpdateUser",
                UserId = userId,
                EmployerId = employerId,
                Firstname = response.Result.FirstName,
                Lastname = response.Result.LastName,
                Email = response.Result.EmailAddress,
                Username = response.Result.UserName
            };

            return PartialView("_EmployerCreateUserSection", viewModel);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(EmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Problem(title: "Model is not valid");
            }

            var command = new CreateUserDto(
                model.Username,
                model.Password,
                model.Firstname,
                model.Lastname,
                Role.EmployerAdmin,
                model.EmployerId,
                "",
                model.Email,
                "", "", "", "");

            var response = await _employerUserClient.CreateEmployerUser(command);

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Title);
                return Problem(title: response.Error.Title);
            }

            return Ok();
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(EmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Problem(title: "Model is not valid");
            }

            if (model.UserId == null)
            {
                ModelState.AddModelError("", "Something went wrong... Try refreshing");
                return BadRequest();
            }

            var command = new PutUserDto(
                model.Username,
                model.Password,
                model.Firstname,
                model.Lastname,
                "",
                model.Email,
                "", "", "", "");

            var response = await _employerUserClient.UpdateEmployerUser(command, model.UserId.Value);

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Title);
                return Problem(title: response.Error.Title);
            }

            return Ok();
        }

        [HttpGet("AllSettings")]
        public async Task<IActionResult> AllSettings(int employerId)
        {
            var response = await _userServiceClient.GetEmployerSettings(employerId);

            if (response == null)
            {
                throw new Exception("Failed to retrieve employer users");
            }

            var viewModel = new EmployerSettingsViewModel
            {
                EmployerId = employerId,
                EmployerSettings = response.ToList()
            };

            return PartialView("_EmployerSettings", viewModel);
        }

        [HttpPatch("AllSettings")]
        public async Task<IActionResult> AllSettings(EmployerSettingsViewModel model)
        {
            var response = await _userServiceClient.UpdateEmployerSettings(model.EmployerId, model.EmployerSettings);

            if (response == null)
            {
                throw new Exception("Failed to retrieve employer settings");
            }

            var viewModel = new EmployerSettingsViewModel
            {
                EmployerId = model.EmployerId,
                EmployerSettings = response.ToList()
            };

            return PartialView("_EmployerSettings", viewModel);
        }
    }
}
       