using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EncryptController : Controller
    {
        private readonly IEncodingService _encodingService;

        public EncryptController(IEncodingService encodingService)
        {
            _encodingService = encodingService;
        }

        [HttpGet("Employer/{employerId}/Employee/{employeeId}")]
        [Authorize(Roles = $"{nameof(Role.EmployerAdmin)}, {nameof(Role.Employee)}")]
        public async Task<IActionResult> EncodeEmployerEmployee(int employerId, int employeeId)
        {
            var response = await _encodingService.GetEncodedEmployerEmployee(employerId, employeeId);

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
