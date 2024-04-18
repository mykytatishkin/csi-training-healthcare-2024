using CSI.IBTA.Shared.Authorization.Types;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Authorization.Constants;
using CSI.IBTA.UserService.Authorization.Extensions;
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
        private readonly IAuthorizationService _authorizationService;

        public EncryptController(IEncodingService encodingService, IAuthorizationService authService)
        {
            _encodingService = encodingService;
            _authorizationService = authService;
        }

        [HttpGet("Employer/{employerId}/Employee/{employeeId}")]
        [Authorize(Roles = $"{nameof(Role.EmployerAdmin)}, {nameof(Role.Employee)}")]
        public async Task<IActionResult> EncodeEmployerEmployee(int employerId, int employeeId)
        {
            var authRes = await _authorizationService.AuthorizeAsync(User, (employeeId, employerId).GetResource(), PolicyConstants.EmployeeOwner);
            if (!authRes.Succeeded) return Forbid();

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

        [HttpGet("Employer/{employerId}/Employee/{employeeId}/Settings")]
        [Authorize(Roles = $"{nameof(Role.EmployerAdmin)}, {nameof(Role.Employee)}")]
        public async Task<IActionResult> EncodeEmployerEmployeeSettings(int employerId, int employeeId)
        {
            var authRes = await _authorizationService.AuthorizeAsync(User, (employeeId, employerId).GetResource(), PolicyConstants.EmployeeOwner);
            if (!authRes.Succeeded) return Forbid();

            var response = await _encodingService.GetEncodedEmployerEmployeeSettings(employerId, employeeId);

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
