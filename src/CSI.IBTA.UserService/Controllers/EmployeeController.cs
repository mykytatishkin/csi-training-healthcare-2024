using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Authorization.Constants;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly IAuthorizationService _authorizationService;

        public EmployeeController(IEmployeesService employeesService, IAuthorizationService authorizationService)
        {
            _employeesService = employeesService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEmployees(
            int page,
            int pageSize,
            int employerId,
            string firstname = "",
            string lastname = "",
            string ssn = "")
        {
            var result = await _authorizationService.AuthorizeAsync(User, employerId, PolicyConstants.EmployerAdminOwner);

            if (!result.Succeeded) return Forbid();

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
    }
}
