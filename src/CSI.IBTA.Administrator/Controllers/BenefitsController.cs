using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("Benefits")]
    public class BenefitsController : Controller
    {

        private readonly IBenefitsServiceClient _benefitsServiceClient;
        private readonly IUserServiceClient _userClient;

        public BenefitsController(
            IBenefitsServiceClient benefitsServiceClient,
            IUserServiceClient userClient)
        {
            _benefitsServiceClient = benefitsServiceClient;
            _userClient = userClient;
        }

        [HttpGet("InsurancePackages")]
        public async Task<IActionResult> InsurancePackages(int employerId)
        {
            var res = await _benefitsServiceClient.GetInsurancePackages(employerId);
            
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to retrieve employer's packages");
            }

            var viewModel = new InsurancePackageViewModel
            {
                InsurancePackages = res.Result,
                EmployerId = employerId
            };

            return PartialView("_EmployerPackagesMenu", viewModel);
        }

        [HttpPatch("InitializePackage")]
        public async Task<IActionResult> InitializePackage(int employerId, int packageId)
        {
            var res = await _benefitsServiceClient.InitializeInsurancePackage(packageId);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to initialize insurance package");
            }

            return await InsurancePackages(employerId);
        }

        [HttpDelete("RemovePackage")]
        public async Task<IActionResult> RemovePackage(int employerId, int packageId)
        {
            var res = await _benefitsServiceClient.RemoveInsurancePackage(packageId);
            if (res.Error != null || res.Result == false)
            {
                throw new Exception("Failed to remove insurance package");
            }

            return await InsurancePackages(employerId);
        }

        [HttpGet("EditClaim")]
        public async Task<IActionResult> EditClaim(int claimId)
        {
            var res = await _benefitsServiceClient.GetClaim(claimId);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to retrieve insurance claim");
            }

            var user = await _userClient.GetUser(res.Result.EmployeeId);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to retrieve claim user");
            }

            //var getPlansResponse = await _benefitsServiceClient.GetPlans();
            //if (getPlansResponse.Result == null)
            //{
            //    return Problem(title: "Failed to retrieve plans");
            //}
            var Plans = new List<PlanIdAndNameDto>();
            Plans.Add(new PlanIdAndNameDto(1, 1.ToString()));
            Plans.Add(new PlanIdAndNameDto(2, 2.ToString()));

            //var plan = getPlansResponse.Result.FirstOrDefault(x => x.Id == res.Result.PlanId);
            var plan = Plans.FirstOrDefault(x => x.Id == res.Result.PlanId);

            var model = new EditClaimViewModel()
            {
                ClaimId = res.Result.Id,
                ClaimNumber = res.Result.ClaimNumber,
                DateOfService = res.Result.DateOfService,
                Amount = res.Result.Amount,
                Employee = new UserInfoDto(res.Result.EmployeeId, user.Result.FirstName, user.Result.LastName, user.Result.PhoneNumber),
                PlanId = plan.Id,
                Plan = plan,
                //AvailablePlans = getPlansResponse.Result.ToList()
                AvailablePlans = Plans.ToList()
            };

            return PartialView("_ClaimEditMenu", model);
        }
        [HttpPatch("EditClaim")]
        public async Task<IActionResult> EditClaim(EditClaimViewModel model)
        {
            //var getPlanResponse = await _benefitsServiceClient.GetPlan(model.Plan.Id);
            //if (getPlanResponse.Result == null)
            //{
            //    return Problem(title: "Failed to retrieve plan");
            //}

            var updateClaimDto = new UpdateClaimDto(model.DateOfService, model.PlanId, model.Amount);
            var res = await _benefitsServiceClient.UpdateClaim(model.ClaimId, updateClaimDto);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to edit insurance claim");
            }

            return PartialView("_claims");
        }
    }
}
