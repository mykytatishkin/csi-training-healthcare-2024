using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using System.Xml.Linq;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("InsurancePlans")]
    public class InsurancePlanController : Controller
    {
        private readonly IPlansClient _plansClient;
        private readonly IUserServiceClient _userClient;

        public InsurancePlanController(
            IPlansClient plansClient,
            IUserServiceClient userClient)
        {
            _plansClient = plansClient;
            _userClient = userClient;
        }

        [HttpPost("OpenAddPlanToListForm")]
        public IActionResult OpenAddPlanToListForm(InsurancePackageCreationViewModel model)
        {
            InsurancePackageNewPlanViewModel planModel = new()
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

            var newPlan = new PlanDto(0,
                model.Name,
                new PlanTypeDto(model.PlanType.Id, model.PlanType.Name),
                model.Contribution,
                0);

            if (model.PackageModel.Package.Plans != null && model.PackageModel.Package.Plans.Count != 0)
            {
                model.PackageModel.Plans.AddRange(model.PackageModel.Package.Plans.ConvertAll(x => new PlanDto(0, x.Name, new PlanTypeDto(x.PlanTypeId, "Medical"), x.Contribution, 0)));
            }

            model.PackageModel.Plans.Add(newPlan);

            return PartialView("InsurancePackages/_CreateInsurancePackage", model.PackageModel);
        }

        [HttpPost("OpenUpdatePlanToListForm")]
        public IActionResult OpenUpdatePlanToListForm(InsurancePackageModificationViewModel model)
        {
            InsurancePackageUpdatePlanViewModel planModel = new()
            {
                PackageModel = model,
                EmployerId = model.EmployerId,
                PlanType = model.AvailablePlanTypes.FirstOrDefault(t => t.Id == model.SelectedPlanTypeId),
            };
            return PartialView("_InsurancePackagePlanUpdateToList", planModel);
        }

        [HttpPost("UpdatePlanToList")]
        public async Task<IActionResult> UpdatePlanToList(InsurancePackageUpdatePlanViewModel model)
        {
            try
            {
                var getEmployerResponse = await _userClient.GetEmployerById(model.EmployerId);
                if (getEmployerResponse.Error != null)
                {
                    return Problem(title: "Failed to retrieve employer");
                }

                var newPlan = new PlanDto(
                    0,
                    model.Name,
                    model.PlanType,
                    model.Contribution,
                    model.PackageModel.Package.Id);

                //model.PackageModel.Plans = model.PackageModel.Package.Plans;
                model.PackageModel.Plans.Add(newPlan);

                model.PackageModel = new InsurancePackageModificationViewModel()
                {
                    EmployerId = model.EmployerId,
                    Package = new FullInsurancePackageDto(model.PackageModel.Package.Id, model.PackageModel.Package.Name, model.PackageModel.Package.PlanStart, model.PackageModel.Package.PlanEnd, model.PackageModel.Package.PayrollFrequency, model.EmployerId, model.PackageModel.Plans.Select(x => new PlanDto(x.Id, x.Name, new PlanTypeDto(x.Id, x.Name), x.Contribution, x.PackageId)).ToList()),
                    Plans = model.PackageModel.Plans,
                    SelectedPlanTypeId = model.PackageModel.SelectedPlanTypeId,
                    AvailablePlanTypes = model.PackageModel.AvailablePlanTypes
                };

                return PartialView("InsurancePackages/_ModifyInsurancePackage", model.PackageModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { error = true, message = e.Message });
            }
        }

        [HttpGet("CreatePlan")]
        public async Task<IActionResult> CreatePlan(int employerId, List<PlanDto> plans)
        {
            var getPlanTypesResponse = await _plansClient.GetPlanTypes();
            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }
            var PlanTypes = getPlanTypesResponse.Result;

            InsurancePackagePlanViewModel model = new InsurancePackagePlanViewModel()
            {
                ActionName = "CreatePlan",
                EmployerId = employerId,
                AvailablePlanTypes = PlanTypes.Select(x => new PlanTypeDto(x.Id, x.Name)).ToList(),
                Plans = plans
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
            var response = await _plansClient.CreatePlan(planDto);
            return PartialView("_EmployerAdministrationMenu", model.EmployerId);
        }

        [HttpGet("UpdatePlan")]
        public async Task<IActionResult> UpdatePlan(int employerId, int planId)
        {
            var response = await _plansClient.GetPlan(planId);
            if (response.Result == null)
            {
                return Problem(title: "Failed to retrieve plan");
            }
            var getPlanTypesResponse = await _plansClient.GetPlanTypes();
            if (getPlanTypesResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve plan types");
            }
            var plan = response.Result;
            var PlanTypes = getPlanTypesResponse.Result;

            InsurancePackagePlanViewModel model = new()
            {
                ActionName = "UpdatePlan",
                PlanId = plan.Id,
                Name = plan.Name,
                EmployerId = employerId,
                PlanTypeId = plan.PlanType.Id,
                Contribution = plan.Contribution,
                AvailablePlanTypes = PlanTypes.Select(x => new PlanTypeDto(x.Id, x.Name)).ToList()
            };
            return PartialView("_InsurancePackagePlanUpdate", model);
        }

        [HttpPost("UpdatePlan")]
        public async Task<IActionResult> UpdatePlan(InsurancePackagePlanViewModel model)
        {
            var getEmployerResponse = await _userClient.GetEmployerById(model.EmployerId);
            if (getEmployerResponse.Error != null)
            {
                return Problem(title: "Failed to retrieve employer");
            }

            var planDto = new UpdatePlanDto(model.Name, model.Contribution, new PlanTypeDto(model.PlanTypeId, ""));
            var response = await _plansClient.UpdatePlan((int)model.PlanId, planDto);
            return PartialView("_EmployerAdministrationMenu", model.EmployerId);
        }
    }
}