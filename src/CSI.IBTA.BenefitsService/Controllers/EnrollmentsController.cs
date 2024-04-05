using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Extensions;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentsService _enrollmentsService;

        public EnrollmentsController(IEnrollmentsService enrollmentsService)
        {
            _enrollmentsService = enrollmentsService;
        }

        [HttpPost("GetByUserIds")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetUsersEnrollments(List<int> userIds)
        {
            var response = await _enrollmentsService.GetUsersEnrollments(userIds);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPost("{employeeId}")]
        [Authorize(Roles = $"{nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetEnrollmentsByEmployeeId(int employeeId, GetEnrollmentsDto dto)
        {
            var employerId = User.GetEmployerId();
            
            if(employerId == null) return Problem(title: "EmployerId claim not found or invalid");

            var response = await _enrollmentsService.GetEnrollmentsByEmployeeId(employeeId, (int) employerId, dto.EncodedEmployerEmployee);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPut("Employer/{employerId}/Employee/{employeeId}")]
        [Authorize(Roles = $"{nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> UpsertEnrollments(int employerId, int employeeId, UpsertEnrollmentsDto dto)
        {
            var response = await _enrollmentsService.UpsertEnrollments(employerId, employeeId, dto.EncodedEmployerEmployee, dto.Enrollments);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }
    }
}
