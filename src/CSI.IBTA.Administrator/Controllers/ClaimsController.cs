using CSI.IBTA.Administrator.Clients;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Claims")]
    public class ClaimsController : Controller
    {
        private readonly IClaimsClient _claimsClient;
        private readonly IUserServiceClient _usersClient;
        private readonly IPlansClient _plansClient;

        public ClaimsController(IClaimsClient claimsClient, IUserServiceClient usersClient, IPlansClient plansClient)
        {
            _claimsClient = claimsClient;
            _usersClient = usersClient;
            _plansClient = plansClient;
        }

        [HttpGet("Details")]
        public async Task<IActionResult> ClaimDetails(int claimId)
        {
            var claimsRes = await _claimsClient.GetClaim(claimId);

            if (claimsRes.Result == null)
            {
                throw new Exception("Failed to retrieve claim details");
            }

            var userRes = await _usersClient.GetUser(claimsRes.Result.EmployeeId);
            if (userRes.Result == null)
            {
                throw new Exception("Failed to retrieve claim details");
            }

            var viewModel = new ClaimDetailsViewModel
            {
                Claim = claimsRes.Result,
                Consumer = userRes.Result
            };

            return PartialView("_ClaimDetails", viewModel);
        }

        [HttpPatch("Approve/{claimId}")]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var res = await _claimsClient.ApproveClaim(claimId);
            return Json(res);
        }

        [HttpPatch("Deny/{claimId}")]
        public async Task<IActionResult> DenyClaim(int claimId, DenyClaimDto dto)
        {
            var res = await _claimsClient.DenyClaim(claimId, dto);
            return Json(res);
        }

        [HttpPost("OpenEditClaim")]
        public async Task<IActionResult> EditClaim(ClaimDetailsViewModel claimModel)
        {
            var getPlansResponse = await _plansClient.GetPlans(claimModel.Consumer.Id);
            if (getPlansResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plans");
            }
            var Plans = getPlansResponse.Result;

            var model = new EditClaimViewModel()
            {
                Claim = claimModel.Claim,
                Consumer = claimModel.Consumer,
                AvailablePlans = Plans.ToList()
            };

            return PartialView("_ClaimEditMenu", model);
        }

        [HttpPatch("EditClaim")]
        public async Task<IActionResult> EditClaim(EditClaimViewModel claimModel)
        {
            var getPlanResponse = await _plansClient.GetPlan(claimModel.Claim.PlanId);
            if (getPlanResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan");
            }
            var plan = getPlanResponse.Result;

            claimModel.Claim = new ClaimDto(claimModel.Claim.Id, claimModel.Claim.EmployeeId,
                claimModel.Claim.EmployerId, claimModel.Claim.PlanId, claimModel.Claim.ClaimNumber,
                claimModel.Claim.DateOfService, plan.Name, plan.PlanType.Name,
                claimModel.Claim.Amount, claimModel.Claim.Status, claimModel.Claim.RejectionReason, claimModel.Claim.Receipt);

            var updateClaimDto = new UpdateClaimDto(claimModel.Claim.DateOfService, claimModel.Claim.PlanId, claimModel.Claim.Amount);
            var res = await _claimsClient.UpdateClaim(claimModel.Claim.Id, updateClaimDto);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: res.Error.Title);
            }

            var viewModel = new ClaimDetailsViewModel
            {
                Claim = claimModel.Claim,
                Consumer = claimModel.Consumer
            };

            return PartialView("_ClaimDetails", viewModel);
        }
    }
}
