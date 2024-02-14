using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployerController : Controller
    {
        private readonly IEmployersService _employerService;

        public EmployerController(IEmployersService employerService)
        {
            _employerService = employerService;
        }

        [HttpGet("{employerId}")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetEmployer(int employerId)
        {
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

        [HttpPut("{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
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
    }
}
