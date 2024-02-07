using CSI.IBTA.AuthService.DTOs;
using CSI.IBTA.AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.AuthService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authenticationService.Login(request);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
