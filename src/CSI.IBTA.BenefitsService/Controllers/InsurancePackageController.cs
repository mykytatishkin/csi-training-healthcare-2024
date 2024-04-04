using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Constants;
using CSI.IBTA.BenefitsService.Extensions;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InsurancePackageController : Controller
    {
        private readonly IInsurancePackageService _insurancePackageService;

        public InsurancePackageController(IInsurancePackageService insurancePackageService)
        {
            _insurancePackageService = insurancePackageService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> CreateInsurancePackage(CreateInsurancePackageDto dto)
        {
            var response = await _insurancePackageService.CreateInsurancePackage(dto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("GetByEmployer/{employerId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetInsurancePackages(int employerId)
        {
            var response = await _insurancePackageService.GetInsurancePackages(employerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("GetFullByEmployer")]
        [Authorize(Roles = nameof(Role.EmployerAdmin))]
        public async Task<IActionResult> GetFullInsurancePackages()
        {
            var employerId = User.GetEmployerId();

            if (employerId == null) return Problem(title: "EmployerId claim not found or invalid");

            var response = await _insurancePackageService.GetFullInsurancePackages((int) employerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("{packageId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetInsurancePackage(int packageId)
        {
            var response = await _insurancePackageService.GetInsurancePackage(packageId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPatch("{packageId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> InitializeInsurancePackage(int packageId)
        {
            var response = await _insurancePackageService.InitializeInsurancePackage(packageId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPut("{packageId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> UpdateInsurancePackage(UpdateInsurancePackageDto dto, int packageId)
        {
            var response = await _insurancePackageService.UpdateInsurancePackage(dto, packageId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }
        
        [HttpDelete("{packageId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> RemoveInsurancePackage(int packageId)
        {
            var response = await _insurancePackageService.RemoveInsurancePackage(packageId);

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