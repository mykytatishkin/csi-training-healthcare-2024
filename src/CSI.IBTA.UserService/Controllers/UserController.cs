﻿using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Administrator)}, {nameof(Role.EmployerAdmin)}")]
        public async Task<IActionResult> GetAllUsers(int accountId)
        {
            var response = await _userService.GetAllUsers(HttpContext);

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
            var response = await _userService.GetUser(accountId, HttpContext);

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
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var response = await _userService.CreateUser(createUserDto, HttpContext);

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
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var response = await _userService.UpdateUser(userId, updateUserDto, HttpContext);

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
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var response = await _userService.DeleteUser(userId, HttpContext);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return NoContent();
        }
    }
}