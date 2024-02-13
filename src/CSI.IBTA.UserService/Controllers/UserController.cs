using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;

        public UserController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int accountId)
        {
            var response = await _userService.GetUser(accountId, HttpContext);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var response = await _userService.CreateUser(createUserDto, HttpContext);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var response = await _userService.UpdateUser(userId, updateUserDto, HttpContext);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var response = await _userService.DeleteUser(userId, HttpContext);

            if (response == false)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
