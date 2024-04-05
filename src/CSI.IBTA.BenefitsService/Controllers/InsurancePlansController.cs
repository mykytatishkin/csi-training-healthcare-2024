using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InsurancePlansController : Controller
    {
        private readonly IInsurancePlanService _benefitsService;

        public InsurancePlansController(IInsurancePlanService benefitsService)
        {
            _benefitsService = benefitsService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetAllPlans(int? customerId)
        {
            var response = await _benefitsService.GetAllPlans(customerId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPost("~/api/v1/ActivePlansByNames")]
        [Authorize]
        public async Task<IActionResult> GetActivePlansByNames(List<string> planNames)
        {
            var response = await _benefitsService.GetActivePlansByNames(planNames);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("{planId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> GetPlan(int planId)
        {
            var response = await _benefitsService.GetPlan(planId);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpGet("PlanTypes")]
        [Authorize]
        public async Task<IActionResult> GetPlanTypes()
        {
            var response = await _benefitsService.GetPlanTypes();

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
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> CreatePlan(int packageId,CreatePlanDto createPlanDto)
        {
            var response = await _benefitsService.CreatePlan(packageId, createPlanDto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }

        [HttpPatch("{planId}")]
        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> UpdatePlan(int planId, UpdatePlanDto updatePlanDto)
        {
            var response = await _benefitsService.UpdatePlan(planId, updatePlanDto);

            if (response.Error != null)
            {
                return Problem(
                    title: response.Error!.Title,
                    statusCode: (int)response.Error.StatusCode
                );
            }

            return Ok(response.Result);
        }
    }
}
