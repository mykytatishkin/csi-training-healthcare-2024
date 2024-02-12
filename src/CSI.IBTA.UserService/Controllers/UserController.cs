using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(Name = "GetUser")]
        public async Task<IActionResult> GetUser(AccountDto accountData)
        {
            var response = await _userService.GetUser(request);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
