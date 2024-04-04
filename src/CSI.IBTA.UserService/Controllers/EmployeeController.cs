using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;

        public EmployeeController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetEmployees(
            int page,
            int pageSize,
            int employerId,
            string firstname = "",
            string lastname = "",
            string ssn = "")
        {
            var response = await _employeesService.GetEmployees(page, pageSize, employerId, firstname, lastname, ssn);

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
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var response = await _employeesService.CreateEmployee(dto);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error!.Title);
            }

            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var response = await _employeesService.GetEmployee(id);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error.Title);
            }

            return Ok(response.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeDto dto)
        {
            var response = await _employeesService.UpdateEmployee(id, dto);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error.Title);
            }

            return Ok(response.Result);
        }
    }
}