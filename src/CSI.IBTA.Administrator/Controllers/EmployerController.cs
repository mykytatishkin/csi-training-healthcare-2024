using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Employer")]
    public class EmployerController : Controller
    {
        private readonly IEmployerClient _employerClient;
        private readonly IEmployerUserClient _employerUserClient;
        private readonly IUserServiceClient _userServiceClient;
        private readonly IJwtTokenService _jwtTokenService;

        public EmployerController(
            IEmployerClient employerClient,
            IEmployerUserClient employerUserClient,
            IJwtTokenService jwtTokenService,
            IUserServiceClient userServiceClient)
        {
            _employerClient = employerClient;
            _employerUserClient = employerUserClient;
            _jwtTokenService = jwtTokenService;
            _userServiceClient = userServiceClient;
        }

        public async Task<IActionResult> Index(int employerId)
        {
            var response = await _employerClient.GetEmployerById(employerId);

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

        [HttpGet("Users")]
        public async Task<IActionResult> Users(int employerId)
        {
            var response = await _employerUserClient.GetEmployerUsers(employerId);

            if (response.Error != null || response.Result == null)
            {
                throw new Exception("Failed to retrieve employer users");
            }

            var viewModel = new UserManagementViewModel
            {
                EmployerId = employerId,
                EmployerUsers = response.Result,
                CreateEmployerUserVM = new EmployerUserViewModel()
            };

            return PartialView("_EmployerAdministrationUserManagement", viewModel);
        }

        [HttpPost("User")]
        public async Task<IActionResult> CreateUser(int employerId, UserManagementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return PartialView("_EmployerAdministrationUserManagement", model);
                return RedirectToAction("Index", "Home");
            }

            var command = new CreateUserDto(
                model.CreateEmployerUserVM.Username,
                model.CreateEmployerUserVM.Password,
                model.CreateEmployerUserVM.Firstname,
                model.CreateEmployerUserVM.Lastname,
                Role.EmployerAdmin,
                employerId,
                "",
                model.CreateEmployerUserVM.Email,
                "", "", "", "");

            var response = await _employerUserClient.CreateEmployerUser(command);

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Title);
            }

            //return PartialView("_EmployerAdministrationUserManagement", model);
            return RedirectToAction("Index", "Home");
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
                throw new Exception("Failed to retrieve user");
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
                //return PartialView("_EmployerAdministrationUserManagement", model);
                return RedirectToAction("Index", "Home");
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
            }

            //return PartialView("_EmployerAdministrationUserManagement", model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(EmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return PartialView("_EmployerAdministrationUserManagement", model);
                return RedirectToAction("Index", "Home");
            }

            if (model.UserId == null)
            {
                ModelState.AddModelError("", "Something went wrong... Try refreshing");
                return PartialView("_EmployerCreateUserSection", model);
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
            }

            //return PartialView("_EmployerAdministrationUserManagement", model);
            return RedirectToAction("Index", "Home");
        }
    }
}
