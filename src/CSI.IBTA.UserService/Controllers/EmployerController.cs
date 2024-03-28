using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Authorization.Constants;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployerController : Controller
    {
        private readonly IEmployersService _employerService;
        private readonly IAuthorizationService _authorizationService;

        public EmployerController(IEmployersService employerService, IAuthorizationService authorizationService)
        {
            _employerService = employerService;
            _authorizationService = authorizationService;
        }

        [HttpGet("{employerId}")]
        [Authorize]
        public async Task<IActionResult> GetEmployer(int employerId)
        {
            var result = await _authorizationService.AuthorizeAsync(User, employerId, PolicyConstants.EmployerAdminOwner);

            if (!result.Succeeded) return Forbid();

            var response = await _employerService.GetEmployer(employerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("GetByAccountId/{accountId}")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetEmployerByAccountId(int accountId)
        {
            var response = await _employerService.GetEmployerByAccountId(accountId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("~/api/v1/Employers")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}")]
        public async Task<IActionResult> GetEmployer([FromQuery] List<int> employerIds)
        {
            var response = await _employerService.GetEmployers(employerIds);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> CreateEmployer([FromForm] CreateEmployerDto dto)
        {
            var response = await _employerService.CreateEmployer(dto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GelAllEmployers()
        {
            var response = await _employerService.GetAll();

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("Filtered")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GelEmployersFiltered(int page = 1, int pageSize = 8, string nameFilter = "", string codeFilter = "")
        {
            var response = await _employerService.GetEmployersFiltered(page, pageSize, nameFilter, codeFilter);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPut("{employerId}")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> UpdateEmployer(int employerId, [FromForm] UpdateEmployerDto dto)
        {
            var response = await _employerService.UpdateEmployer(employerId, dto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpDelete("{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> DeleteUser(int employerId)
        {
            var response = await _employerService.DeleteEmployer(employerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return NoContent();
        }

        [HttpGet("settings/{employerId}")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetEmployerSetting(int employerId, string condition)
        {
            var response = await _employerService.GetEmployerSettingValue(employerId, condition);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("allsettings/{employerId}")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetAllEmployerSettings(int employerId)
        {
            var response = await _employerService.GetAllEmployerSettings(employerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPatch("allsettings/{employerId}")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> UpdateAllEmployerSettings(int employerId, SettingsDto[] SettingsDtos)
        {
            var response = await _employerService.UpdateEmployerSettings(employerId, SettingsDtos);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("{employerId}/Users")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetEmployerUsers(int employerId)
        {
            var employerUsersResponse = await _employerService.GetEmployerUsers(employerId);

            if (employerUsersResponse.Error != null)
            {
                return Problem(
                    title: employerUsersResponse.Error.Title,
                    statusCode: (int)employerUsersResponse.Error.StatusCode
                );
            }

            return Ok(employerUsersResponse.Result);
        }
    }
}