using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int accountId)
        {
            var response = await _userService.GetUserByAccountId(accountId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            if (!IsNextSuperiorRole(HttpContext.User, createUserDto.Role))
            {
                return Unauthorized("Invalid User Role");
            }

            var response = await _userService.CreateUser(createUserDto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPatch("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var getResponse = await _userService.GetUser(userId);
            if (getResponse.Error != null)
            {
                return Problem(
                    title: getResponse.Error!.Title,
                    statusCode: (int)getResponse.Error.StatusCode
                );
            }
            var authUserId = (HttpContext.User).FindFirstValue(ClaimTypes.NameIdentifier);
            if (authUserId == null || (int.Parse(authUserId) != getResponse.Result.AccountId
                && !IsNextSuperiorRole(HttpContext.User, getResponse.Result.Role)))
            {
                return Unauthorized("User is unauthorized");
            }

            var response = await _userService.UpdateUser(userId, updateUserDto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var getResponse = await _userService.GetUser(userId);
            if (getResponse.Error != null)
            {
                return Problem(
                    title: getResponse.Error!.Title,
                    statusCode: (int)getResponse.Error.StatusCode
                );
            }
            if (!IsNextSuperiorRole(HttpContext.User, getResponse.Result.Role))
            {
                return Unauthorized("User is unauthorized");
            }

            var response = await _userService.DeleteUser(userId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return NoContent();
        }

        private bool IsNextSuperiorRole(Role authUserRole, Role managedUserRole)
        {
            return (authUserRole == Role.Administrator && managedUserRole == Role.EmployerAdmin)
                || (authUserRole == Role.EmployerAdmin && managedUserRole == Role.Employee);
        }

        private bool IsNextSuperiorRole(ClaimsPrincipal authenticatedUser, Role managedUserRole)
        {
            Enum.TryParse(authenticatedUser.FindFirstValue(ClaimTypes.Role), out Role authUserRole);
            return IsNextSuperiorRole(authUserRole, managedUserRole);
        }

        private bool IsSuperiorRole(ClaimsPrincipal authenticatedUser, Role managedUserRole)
        {
            Enum.TryParse(authenticatedUser.FindFirstValue(ClaimTypes.Role), out Role authUserRole);
            return managedUserRole != Role.Administrator &&
                (IsNextSuperiorRole(authUserRole, managedUserRole)
                || authUserRole == Role.Administrator);
        }
    }
}
