using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.BenefitsService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        //[Authorize]
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

        //[HttpGet("{userId}")]
        //[Authorize]
        //public async Task<IActionResult> GetUser(int userId)
        //{
        //    var response = await _benefitsService.GetUser(userId);

        //    if (response.Error != null)
        //    {
        //        return Problem(
        //            title: response.Error!.Title,
        //            statusCode: (int)response.Error.StatusCode
        //        );
        //    }

        //    return Ok(response.Result);
        //}

        [HttpPost]
        //[Authorize]
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

        //[HttpPut("{userId}")]
        //// Later we can have policy based authorization which will handle checking
        //// if user is owner of the resource
        //[Authorize(Roles = nameof(Role.Administrator))]
        //public async Task<IActionResult> PutUser(int userId, PutUserDto putUserDto)
        //{
        //    var getResponse = await _benefitsService.GetUser(userId);
        //    if (getResponse.Error != null)
        //    {
        //        return Problem(
        //            title: getResponse.Error!.Title,
        //            statusCode: (int)getResponse.Error.StatusCode
        //        );
        //    }

        //    var response = await _benefitsService.PutUser(userId, putUserDto);

        //    if (response.Error != null)
        //    {
        //        return Problem(
        //            title: response.Error!.Title,
        //            statusCode: (int)response.Error.StatusCode
        //        );
        //    }

        //    return Ok(response.Result);
        //}

        //[HttpDelete("{userId}")]
        //[Authorize]
        //public async Task<IActionResult> DeleteUser(int userId)
        //{
        //    var getResponse = await _benefitsService.GetUser(userId);
        //    if (getResponse.Error != null)
        //    {
        //        return Problem(
        //            title: getResponse.Error!.Title,
        //            statusCode: (int)getResponse.Error.StatusCode
        //        );
        //    }
        //    if (!IsNextSuperiorRole(HttpContext.User, getResponse.Result.Role))
        //    {
        //        return Unauthorized("User is unauthorized");
        //    }

        //    var response = await _benefitsService.DeleteUser(userId);

        //    if (response.Error != null)
        //    {
        //        return Problem(
        //            title: response.Error!.Title,
        //            statusCode: (int)response.Error.StatusCode
        //        );
        //    }

        //    return NoContent();
        //}
    }
}
