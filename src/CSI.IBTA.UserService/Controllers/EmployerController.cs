using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
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

        public EmployerController(IEmployersService employerService)
        {
            _employerService = employerService;
        }

        [HttpGet("{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetEmployerProfile(int employerId)
        {
            var response = await _employerService.GetEmployerProfile(employerId);

            if (response.value == null)
            {
                return NotFound(response.description);
            }

            return Ok(response.value);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> CreateEmployer([FromForm] CreateEmployerDto dto)
        {
            var response = await _employerService.CreateEmployer(dto);

            if (response.value == null)
            {
                return BadRequest(response.description);
            }

            return Ok(response.value);
        }

        [HttpPut("{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> UpdateEmployer(int employerId, [FromForm] UpdateEmployerDto dto)
        {
            var response = await _employerService.UpdateEmployer(employerId, dto);

            if (response.value == null)
            {
                return BadRequest(response.description);
            }

            return Ok(response.value);
        }

        [HttpDelete("{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> DeleteUser(int employerId)
        {
            var response = await _employerService.DeleteEmployer(employerId);

            if (response.value != true)
            {
                return BadRequest(response.description);
            }

            return NoContent();
        }
    }
}
