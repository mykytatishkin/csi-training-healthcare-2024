using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Clients
{
    internal class SettingsClient : ISettingsClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public SettingsClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericResponse<SettingsWithEmployerStateDto>> GetEmployerSetting(
            int employerId, string settingCondition)
        {
            var requestUrl = string.Format(UserServiceEndpoints.GetEmployerSetting, employerId, EmployerConstants.ClaimFilling);
            var response = await _httpClient.GetAsync(requestUrl);
            var claimFillingSetting = await response.Content.ReadFromJsonAsync<SettingsWithEmployerStateDto>();
            response.EnsureSuccessStatusCode();
            return new GenericResponse<SettingsWithEmployerStateDto>(null, claimFillingSetting);
        }

        public async Task<GenericResponse<SettingsWithEmployerStateDto>> UpdateClaimSetting(
            int employerId, UpdateClaimSettingDto updateClaimSettingDto)
        {
            var requestUrl = string.Format(UserServiceEndpoints.UpdateEmployerClaimSetting, employerId);
            var requestContent = JsonContent.Create(updateClaimSettingDto);

            var response = await _httpClient.PutAsync(requestUrl, requestContent);
            var claimFillingSetting = await response.Content.ReadFromJsonAsync<SettingsWithEmployerStateDto>();
            response.EnsureSuccessStatusCode();
            return new GenericResponse<SettingsWithEmployerStateDto>(null, claimFillingSetting);
        }
    }
}