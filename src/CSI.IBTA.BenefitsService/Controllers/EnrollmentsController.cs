using CSI.IBTA.BenefitsService.Interfaces;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEnrollmentsByEnployeeId(int employeeId)
        {
            var response = await _enrollmentsService.GetEnrollmentsByEmployeeId(employeeId);

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
        public async Task<IActionResult> UpsertEnrollments(int employerId, int employeeId, List<UpsertEnrollmentDto> enrollments)
        {
            var response = await _enrollmentsService.UpdateEnrollments(employerId, employeeId, enrollments);

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
