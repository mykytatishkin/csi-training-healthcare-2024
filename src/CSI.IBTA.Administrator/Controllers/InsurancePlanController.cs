using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("InsurancePlans")]
    public class InsurancePlanController : Controller
    {
        private readonly IBenefitsClient _benefitsClient;
        private readonly IUserServiceClient _userClient;

        public InsurancePlanController(
            IBenefitsClient benefitsClient,
            IUserServiceClient userClient)
        {
            _benefitsClient = benefitsClient;
            _userClient = userClient;
        }

        [HttpPost("OpenAddPlanToListForm")]
        public IActionResult OpenAddPlanToListForm(InsurancePackageCreationViewModel model)
        {

            InsurancePackageNewPlanViewModel planModel = new InsurancePackageNewPlanViewModel()
            {
                PackageModel = model,
                EmployerId = model.EmployerId,
                PlanType = model.AvailablePlanTypes.FirstOrDefault(t => t.Id == model.SelectedPlanTypeId),
            };
            return PartialView("_InsurancePackagePlanAddToList", planModel);
        }

        [HttpPost("AddPlanToList")]
        public async Task<IActionResult> AddPlanToList(InsurancePackageNewPlanViewModel model)
        {
            var getEmployerResponse = await _userClient.GetEmployerById(model.EmployerId);
            if (getEmployerResponse.Error != null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            var newPlan = new Plan()
            {
                Name = model.Name,
                Package = model.PackageModel.Package,
                TypeId = model.PlanType.Id,
                PlanType = model.PlanType,
                Contribution = model.Contribution
            };

            if (model.PackageModel.Plans == null)
                model.PackageModel.Plans = new List<Plan>();

            model.PackageModel.Plans.Add(newPlan);

            return PartialView("InsurancePackages/_CreateInsurancePackage", model.PackageModel);
        }

        [HttpGet("CreatePlan")]
        public async Task<IActionResult> CreatePlan(int employerId)
        {
            var getPlanTypesResponse = await _benefitsClient.GetPlanTypes();
            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }
            var PlanTypes = getPlanTypesResponse.Result;

            InsurancePackagePlanViewModel model = new InsurancePackagePlanViewModel()
            {
                ActionName = "CreatePlan",
                EmployerId = employerId,
                Package = new Package(),
                AvailablePlanTypes = PlanTypes.Select(x => new PlanType()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
            return PartialView("_InsurancePackagePlanCreate", model);
        }

        [HttpPost("CreatePlan")]
        public async Task<IActionResult> CreatePlan(InsurancePackagePlanViewModel model)
        {
            var getEmployerResponse = await _userClient.GetEmployerById(model.EmployerId);
            if (getEmployerResponse.Error != null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            var planDto = new CreatePlanDto(model.Name, model.Contribution, model.PlanTypeId);
            var response = await _benefitsClient.CreatePlan(planDto);
            return PartialView("_EmployerAdministrationMenu", model.EmployerId);
        }

        [HttpGet("UpdatePlan")]
        public async Task<IActionResult> UpdatePlan(int employerId, int planId)
        {
            var response = await _benefitsClient.GetPlan(planId);
            if (response.Result == null)
            {
                return Problem(title: "Failed to retrieve plan");
            }
            var getPlanTypesResponse = await _benefitsClient.GetPlanTypes();
            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }
            var plan = response.Result;
            var PlanTypes = getPlanTypesResponse.Result;

            InsurancePackagePlanViewModel model = new InsurancePackagePlanViewModel()
            {
                ActionName = "UpdatePlan",
                PlanId = plan.Id,
                Name = plan.Name,
                EmployerId = employerId,
                PlanTypeId = plan.PlanType.Id,
                Contribution = plan.Contribution,
                AvailablePlanTypes = PlanTypes.Select(x => new PlanType()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
            return PartialView("_InsurancePackagePlanCreate", model);
        }

        [HttpPost("UpdatePlan")]
        public async Task<IActionResult> UpdatePlan(InsurancePackagePlanViewModel model)
        {
            var getEmployerResponse = await _userClient.GetEmployerById(model.EmployerId);
            if (getEmployerResponse.Error != null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            var planDto = new UpdatePlanDto(model.Name, model.Contribution, model.PlanTypeId);
            var response = await _benefitsClient.UpdatePlan((int)model.PlanId, planDto);
            return PartialView("_EmployerAdministrationMenu", model.EmployerId);
        }
    }
}