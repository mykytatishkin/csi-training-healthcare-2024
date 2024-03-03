using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.BenefitsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BenefitsController : Controller
    {
        private readonly IBenefitsService _benefitsService;

        public BenefitsController(IBenefitsService benefitsService)
        {
            _benefitsService = benefitsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPlans()
        {
            var response = await _benefitsService.GetAllPlans();

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
        [Authorize]
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

        [HttpGet("Plans")]
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
        [Authorize]
        public async Task<IActionResult> CreatePlan(CreatePlanDto createPlanDto)
        {
            var response = await _benefitsService.CreatePlan(createPlanDto);

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
        [Authorize]
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
