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

        [HttpPost("OpenEditClaim")]
        public async Task<IActionResult> EditClaim(ClaimDetailsViewModel claimModel)
        {

            //var getPlansResponse = await _benefitsServiceClient.GetPlans();
            
            var getPlansResponse = await _benefitsServiceClient.GetPlans(claimModel.Consumer.Id);
            if (getPlansResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plans");
            }
            var Plans = getPlansResponse.Result;

            //var plan = getPlansResponse.Result.FirstOrDefault(x => x.Id == res.Result.PlanId);
            //var plan = Plans.FirstOrDefault(x => x.Id == claimModel.Claim.PlanId);
            var model = new EditClaimViewModel()
            {
                Claim = new ClaimDto(claimModel.Claim.Id, claimModel.Claim.EmployeeId, 
                claimModel.Claim.EmployerId, claimModel.Claim.PlanId, claimModel.Claim.ClaimNumber, 
                claimModel.Claim.DateOfService, claimModel.Claim.PlanName, claimModel.Claim.PlanTypeName, 
                claimModel.Claim.Amount, claimModel.Claim.Status),
                Consumer = claimModel.Consumer,
                //ClaimNumber = res.Result.ClaimNumber,
                //DateOfService = res.Result.DateOfService,
                //Amount = res.Result.Amount,
                //Employee = new UserInfoDto(res.Result.EmployeeId, user.Result.FirstName, user.Result.LastName, user.Result.PhoneNumber),
                //PlanId = plan.Id,
                //AvailablePlans = getPlansResponse.Result.ToList()
                AvailablePlans = Plans.ToList()
            };

            return PartialView("_ClaimEditMenu", model);
        }
        [HttpPatch("EditClaim")]
        public async Task<IActionResult> EditClaim(EditClaimViewModel claimModel)
        {
            var getPlanResponse = await _benefitsServiceClient.GetPlan(claimModel.Claim.PlanId);
            if (getPlanResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan");
            }
            var plan = getPlanResponse.Result;

            claimModel.Claim = new ClaimDto(claimModel.Claim.Id, claimModel.Claim.EmployeeId,
                claimModel.Claim.EmployerId, claimModel.Claim.PlanId, claimModel.Claim.ClaimNumber,
                claimModel.Claim.DateOfService, plan.Name, plan.PlanType.Name,
                claimModel.Claim.Amount, claimModel.Claim.Status);

            var updateClaimDto = new UpdateClaimDto(claimModel.Claim.DateOfService, claimModel.Claim.PlanId, claimModel.Claim.Amount);
            var res = await _benefitsServiceClient.UpdateClaim(claimModel.Claim.Id, updateClaimDto);
            if (res.Error != null || res.Result == null)
            {
                throw new Exception("Failed to edit insurance claim");
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
