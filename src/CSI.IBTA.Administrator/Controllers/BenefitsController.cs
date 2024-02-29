using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    [Route("Benefits")]
    public class BenefitsController : Controller
    {
        private readonly IEmployerClient _employerClient;
        private readonly IEmployerUserClient _employerUserClient;
        private readonly IUserServiceClient _userServiceClient;

        public BenefitsController(
            IEmployerClient employerClient,
            IEmployerUserClient employerUserClient,
            IUserServiceClient userServiceClient)
        {
            _employerClient = employerClient;
            _employerUserClient = employerUserClient;
            _userServiceClient = userServiceClient;
        }

        public IActionResult Index(int employerId)
        {

            return PartialView("InsuranceManagement", employerId);
        }

        [HttpGet("CreatePlan")]
        public IActionResult CreatePlan(int employerId)
        {
            var plan = new Plan { Id = 1 };
            // on update, retrieve plan
            // retrieve plantypes
            // package stub for now
            InsurancePackagePlanViewModel model = new InsurancePackagePlanViewModel()
            {
                ActionName = "CreatePlan",
                PlanId = 4,
                EmployerId = employerId,
                PackageId = 2,
                PlanTypeId = 2,
                Name = "",
                AvailablePlanTypes = new List<PlanType>()
                {
                    new PlanType() { Id = 1, Name = "Medical"},
                    new PlanType() { Id = 2, Name = "Dental"}
                }
            };
            return PartialView("_InsurancePackagePlanCreate", model);
        }

        [HttpPost("CreatePlan")]
        public async Task<IActionResult> CreatePlan(InsurancePackagePlanViewModel model)
        {
            return PartialView("_EmployerAdministrationMenu", model.EmployerId);
        }
    }
}