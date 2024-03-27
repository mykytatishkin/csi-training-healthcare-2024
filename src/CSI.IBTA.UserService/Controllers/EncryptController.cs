using CSI.IBTA.UserService.Interfaces;
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

        [HttpPost("Employer/{employerId}/Employee/{employeeId}")]
        //[Authorize($"{nameof(Role.EmployerAdmin)}")]
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
