﻿using CSI.IBTA.Shared.Authorization.Types;
using CSI.IBTA.Shared.DTOs;
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

        [HttpGet("{employeeId}")]
        [Authorize]
        public async Task<IActionResult> GetEmployee(int employeeId)
        {
            var response = await _employeesService.GetEmployee(employeeId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            var result = await _authorizationService.AuthorizeAsync(User, response.Result, PolicyConstants.EmployeeOwner);

            if (!result.Succeeded) return Forbid();

            return Ok(response.Result);
        }

        [HttpPost("~/api/v1/EmployeesByUsernames")]
        [Authorize]
        public async Task<IActionResult> GetEmployeesByUsernames(List<string> usernames, int employerId)
        {
            var response = await _employeesService.GetEmployeesByUsernames(usernames, employerId);

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
        [Authorize(Roles = nameof(Role.EmployerAdmin))]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var result = await _authorizationService.AuthorizeAsync(User, dto.EmployerId, PolicyConstants.EmployerAdminOwner);

            if (!result.Succeeded) return Forbid();

            var response = await _employeesService.CreateEmployee(dto);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error!.Title);
            }

            return Ok(response.Result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            var userResponse = await _employeesService.GetEmployee(id);
            if (userResponse.Error != null)
            {
                return Problem(
                    statusCode: (int)userResponse.Error.StatusCode,
                    title: userResponse.Error.Title);
            }

            var result = await _authorizationService.AuthorizeAsync(User, userResponse.Result!.EmployerId, PolicyConstants.EmployerAdminOwner);
            if (!result.Succeeded) return Forbid();

            var response = await _employeesService.UpdateEmployee(dto);
            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error.Title);
            }

            return Ok(response.Result);
        }

        [HttpGet("{employeeId}/ClaimFilling")]
        [Authorize]
        public async Task<IActionResult> GetClaimFillingSetting(int employeeId)
        { 
            var result = await _authorizationService.AuthorizeAsync(User, new EmployeeOwnedResource() { EmployeeId = employeeId }, PolicyConstants.EmployeeOwner);

            if (!result.Succeeded) return Forbid();

            var response = await _employeesService.GetAllowClaimFilling(employeeId);

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