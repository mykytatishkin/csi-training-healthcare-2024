using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("GetByUserIds")]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetUsersEnrollments(List<int> userIds)
        {
            var response = await _enrollmentService.GetUsersEnrollments(userIds);

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
