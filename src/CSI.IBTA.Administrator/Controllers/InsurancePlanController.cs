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

        [HttpPost("OpenAddPlanToListForm")]
        public IActionResult OpenAddPlanToListForm(InsurancePackageCreationViewModel model)
        {
            var planModel = new InsurancePackageNewPlanViewModel()
            {
                PackageModel = model,
                EmployerId = model.EmployerId,
                PlanType = model.AvailablePlanTypes.FirstOrDefault(t => t.Id == model.SelectedPlanTypeId),
            };

            return PartialView("InsurancePackages/_CreatePackagePlanForm", planModel);
        }

        [HttpPost("AddPlanToList")]
        public async Task<IActionResult> AddPlanToList(InsurancePackageNewPlanViewModel model)
        {
            var newPlan = new PlanDto(0,
                model.Name,
                new PlanTypeDto(model.PlanType.Id, model.PlanType.Name),
                model.Contribution,
                0);

            if (model.PackageModel.Package.Plans != null && model.PackageModel.Package.Plans.Count != 0)
            {
                model.PackageModel.Plans.AddRange(model.PackageModel.Package.Plans.ConvertAll(x => new PlanDto(0, x.Name, new PlanTypeDto(x.PlanTypeId, ""), x.Contribution, 0)));
            }

            model.PackageModel.Plans.Add(newPlan);

            return PartialView("InsurancePackages/_CreatePackage", model.PackageModel);
        }

        [HttpPut("UpdatePlan")]
        public async Task<IActionResult> UpdatePlan(InsurancePackageNewPlanViewModel model)
        {
            var plan = model.PackageModel.Plans[model.PackageModel.SelectedPlanIndex];
            model.PackageModel.Plans[model.PackageModel.SelectedPlanIndex] = new PlanDto(plan.Id, model.Name, model.PlanType, model.Contribution, plan.PackageId);

            if (model.PackageModel.Package.Plans != null && model.PackageModel.Package.Plans.Count != 0)
            {
                model.PackageModel.Plans.AddRange(model.PackageModel.Package.Plans.ConvertAll(x => new PlanDto(model.PlanId, x.Name, new PlanTypeDto(x.PlanTypeId, ""), x.Contribution, plan.PackageId)));
            }

            return PartialView("InsurancePackages/_CreatePackage", model.PackageModel);
        }

        [HttpPost("OpenUpdatePlanForm")]
        public async Task<IActionResult> OpenUpdatePlanForm(InsurancePackageCreationViewModel model)
        {
            var planIndex = model.SelectedPlanIndex;
            var planModel = new InsurancePackageNewPlanViewModel()
            {
                PackageModel = model,
                EmployerId = model.EmployerId,
                PlanType = model.Plans[planIndex].PlanType,
                Contribution = model.Plans[planIndex].Contribution,
                Name = model.Plans[planIndex].Name,
                PlanId = model.Plans[planIndex].Id
            };

            return PartialView("InsurancePackages/_CreatePackagePlanForm", planModel);
        }

        [HttpPost("OpenUpdatePlanToListForm")]
        public IActionResult OpenUpdatePlanToListForm(InsurancePackageModificationViewModel model)
        {
            var planModel = new InsurancePackageUpdatePlanViewModel()
            {
                PackageModel = model,
                EmployerId = model.EmployerId,
                PlanType = model.AvailablePlanTypes.FirstOrDefault(t => t.Id == model.SelectedPlanTypeId),
            };
            return PartialView("InsurancePackages/_ModifyPackagePlanForm", planModel);
        }

        [HttpPost("HandleCreatePackagePlanFormCancel")]
        public IActionResult HandleCreatePackagePlanFormCancel(InsurancePackageNewPlanViewModel model)
        {
            var viewModel = new InsurancePackageCreationViewModel()
            {
                EmployerId = model.EmployerId,
                Package = model.PackageModel.Package,
                Plans = model.PackageModel.Plans,
                AvailablePlanTypes = model.PackageModel.AvailablePlanTypes
            };

            return PartialView("InsurancePackages/_CreatePackage", viewModel);
        }

        [HttpPost("HandleUpdatePackagePlanFormCancel")]
        public IActionResult HandleUpdatePackagePlanFormCancel(InsurancePackageUpdatePlanViewModel model)
        {
            model.PackageModel = new InsurancePackageModificationViewModel()
            {
                EmployerId = model.EmployerId,
                Package = new FullInsurancePackageDto(model.PackageModel.Package.Id, model.PackageModel.Package.Name, model.PackageModel.Package.PlanStart, model.PackageModel.Package.PlanEnd, model.PackageModel.Package.PayrollFrequency, model.EmployerId, model.PackageModel.Plans.Select(x => new PlanDto(x.Id, x.Name, new PlanTypeDto(x.Id, x.Name), x.Contribution, x.PackageId)).ToList()),
                Plans = model.PackageModel.Plans,
                SelectedPlanTypeId = model.PackageModel.SelectedPlanTypeId,
                AvailablePlanTypes = model.PackageModel.AvailablePlanTypes
            };

            return PartialView("InsurancePackages/_ModifyPackage", model.PackageModel);
        }

        [HttpPost("OpenUpdatePackageUpdatePlanForm")]
        public IActionResult OpenUpdatePackageUpdatePlanForm(InsurancePackageModificationViewModel model)
        {
            var planIndex = model.SelectedPlanIndex;
            var planModel = new InsurancePackageUpdatePlanViewModel()
            {
                PackageModel = model,
                EmployerId = model.EmployerId,
                PlanType = model.Plans[planIndex].PlanType,
                Contribution = model.Plans[planIndex].Contribution,
                Name = model.Plans[planIndex].Name,
                PlanId = model.Plans[planIndex].Id
            };
            return PartialView("InsurancePackages/_ModifyPackagePlanForm", planModel);
        }

        [HttpPost("UpdatePlanToList")]
        public async Task<IActionResult> UpdatePlanToList(InsurancePackageUpdatePlanViewModel model)
        {
            var newPlan = new PlanDto(
                0,
                model.Name,
                model.PlanType,
                model.Contribution,
                model.PackageModel.Package.Id);

            model.PackageModel.Plans.Add(newPlan);

            model.PackageModel = new InsurancePackageModificationViewModel()
            {
                EmployerId = model.EmployerId,
                Package = new FullInsurancePackageDto(model.PackageModel.Package.Id, model.PackageModel.Package.Name, model.PackageModel.Package.PlanStart, model.PackageModel.Package.PlanEnd, model.PackageModel.Package.PayrollFrequency, model.EmployerId, model.PackageModel.Plans.Select(x => new PlanDto(x.Id, x.Name, new PlanTypeDto(x.Id, x.Name), x.Contribution, x.PackageId)).ToList()),
                Plans = model.PackageModel.Plans,
                SelectedPlanTypeId = model.PackageModel.SelectedPlanTypeId,
                AvailablePlanTypes = model.PackageModel.AvailablePlanTypes
            };

            return PartialView("InsurancePackages/_ModifyPackage", model.PackageModel);
        }

        [HttpPut("UpdatePackageUpdatePlan")]
        public async Task<IActionResult> UpdatePackageUpdatePlan(InsurancePackageUpdatePlanViewModel model)
        {
            var plan = model.PackageModel.Plans[model.PackageModel.SelectedPlanIndex];
            model.PackageModel.Plans[model.PackageModel.SelectedPlanIndex] = new PlanDto(plan.Id, model.Name, model.PlanType, model.Contribution, plan.PackageId);

            model.PackageModel = new InsurancePackageModificationViewModel()
            {
                EmployerId = model.EmployerId,
                Package = new FullInsurancePackageDto(model.PackageModel.Package.Id, model.PackageModel.Package.Name, model.PackageModel.Package.PlanStart, model.PackageModel.Package.PlanEnd, model.PackageModel.Package.PayrollFrequency, model.EmployerId,
                model.PackageModel.Plans.Select(x => new PlanDto(x.Id, x.Name, new PlanTypeDto(x.Id, x.Name), x.Contribution, x.PackageId)).ToList()),
                Plans = model.PackageModel.Plans,
                SelectedPlanTypeId = model.PackageModel.SelectedPlanTypeId,
                AvailablePlanTypes = model.PackageModel.AvailablePlanTypes
            };

            return PartialView("InsurancePackages/_ModifyPackage", model.PackageModel);
        }
    }
}