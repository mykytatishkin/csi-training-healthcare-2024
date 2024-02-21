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
        private readonly IJwtTokenService _jwtTokenService;

        public EmployerController(
            IEmployerClient employerClient,
            IEmployerUserClient employerUserClient,
            IJwtTokenService jwtTokenService)
        {
            _employerClient = employerClient;
            _employerUserClient = employerUserClient;
            _jwtTokenService = jwtTokenService;
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

        [HttpGet("Users")]
        public async Task<IActionResult> Users(int employerId)
        {
            var response = await _employerClient.GetEmployerUsers(employerId);

            if (response.Error != null || response.Result == null)
            {
                throw new Exception("Failed to retrieve employer users");
            }

            var viewModel = new UserManagementViewModel
            {
                EmployerId = employerId,
                EmployerUsers = response.Result,
                CreaterEmployerUserVM = new CreateEmployerUserViewModel()
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

            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var command = new CreateUserDto(
                model.CreaterEmployerUserVM.Username,
                model.CreaterEmployerUserVM.Password,
                model.CreaterEmployerUserVM.Firstname,
                model.CreaterEmployerUserVM.Lastname,
                Role.EmployerAdmin,
                employerId,
                "", 
                model.CreaterEmployerUserVM.Email,
                "", "", "", "");

            var response = await _employerUserClient.CreateEmployerUser(command, token);

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Title);
            }

            //return PartialView("_EmployerAdministrationUserManagement", model);
            return RedirectToAction("Index", "Home");
        }
    }
}
