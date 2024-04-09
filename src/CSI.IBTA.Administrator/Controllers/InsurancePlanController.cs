using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("InsurancePlans")]
    public class InsurancePlanController : Controller
    {

        public InsurancePlanController()
        {
        }

        [HttpPost("OpenCreatePlanForm")]
        public IActionResult OpenCreatePlanForm(InsurancePackageFormViewModel model)
        {
            model.PlanForm.PlanType = new PlanTypeDto(model.PlanForm.PlanType.Id, model.AvailablePlanTypes.First(x => x.Id == model.PlanForm.PlanType.Id).Name);
            return PartialView("InsurancePackages/_PackagePlanForm", model);
        }

        [HttpPost("OpenUpdatePlanForm")]
        public IActionResult OpenUpdatePlanForm(InsurancePackageFormViewModel model)
        {
            var plan = model.Package.Plans[model.PlanForm.SelectedPlanIndex ?? 0];
            model.PlanForm.PlanType = new PlanTypeDto(model.PlanForm.PlanType.Id, model.AvailablePlanTypes.First(x => x.Id == model.PlanForm.PlanType.Id).Name);
            model.PlanForm.Name = plan.Name;
            model.PlanForm.Contribution = plan.Contribution;
            model.PlanForm.PlanId = plan.Id;
            return PartialView("InsurancePackages/_PackagePlanForm", model);
        }

        [HttpPost]
        public IActionResult CreatePlan(InsurancePackageFormViewModel model)
        {
            var newPlan = new PlanDto(
                0,
                model.PlanForm.Name,
                model.PlanForm.PlanType,
                model.PlanForm.Contribution,
                model.Package.Id,
                model.EmployerId);

            model.Package.Plans.Add(newPlan);

            ModelState.Clear();
            return PartialView("InsurancePackages/_PackageForm", model);
        }

        [HttpPut]
        public IActionResult UpsertPlan(InsurancePackageFormViewModel model)
        {
            if (model.PlanForm.SelectedPlanIndex == null) return CreatePlan(model);
            var index = (int) model.PlanForm.SelectedPlanIndex;

            var plan = model.Package.Plans[index];
            model.Package.Plans[index] = new PlanDto(plan.Id, model.PlanForm.Name, model.PlanForm.PlanType, model.PlanForm.Contribution, plan.PackageId, model.Package.EmployerId);
            ModelState.Clear();
            return PartialView("InsurancePackages/_PackageForm", model);
        }

        [HttpPost("HandlePackagePlanFormCancel")]
        public IActionResult HandlePackagePlanFormCancel(InsurancePackageFormViewModel model)
        {
            return PartialView("InsurancePackages/_PackageForm", model);
        }
    }
}