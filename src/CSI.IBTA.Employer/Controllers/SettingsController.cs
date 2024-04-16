using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Models;
using CSI.IBTA.Shared.Constants;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("Settings")]
    [TypeFilter(typeof(AuthenticationFilter))]
    public class SettingsController : Controller
    {
        private readonly ISettingsClient _settingsClient;

        public SettingsController(ISettingsClient settingsClient)
        {
            _settingsClient = settingsClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int employerId)
        {
            var res = await _settingsClient.GetEmployerSetting(employerId, EmployerConstants.ClaimFilling);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve claim setting");
            }

            var setting = res.Result;

            var model = new EmployerSettingsViewModel()
            {
                EmployerId = employerId,
                AdminCondition = setting.State,
                FollowAdminCondition = !setting.State || setting.EmployerState == null,
                EmployerAdminCondition = setting.EmployerState ?? true,
            };

            return PartialView("_Settings", model);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClaimSetting(EmployerSettingsViewModel viewModel)
        {
            if (!viewModel.AdminCondition)
                return PartialView("_Settings", viewModel);

            var settingDto = new UpdateClaimSettingDto(viewModel.EmployerAdminCondition);

            var res = await _settingsClient.UpdateClaimSetting(viewModel.EmployerId, settingDto);

            if (res.Error != null || res.Result == null)
            {
                return Problem(title: "Failed to retrieve claim setting");
            }

            var setting = res.Result;

            var model = new EmployerSettingsViewModel()
            {
                EmployerId = viewModel.EmployerId,
                AdminCondition = setting.State,
                FollowAdminCondition = !setting.State || setting.EmployerState == null,
                EmployerAdminCondition = setting.EmployerState ?? true,
            };

            return PartialView("_Settings", model);
        }
    }
}
