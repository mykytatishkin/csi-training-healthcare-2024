using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Net.Http;

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

        public async Task<GenericResponse<SettingsWithEmployerStateDto>> GetClaimSetting(
            int employerId)
        {
            var requestUrl = string.Format(EmployerEndpoints.GetEmployerSetting, employerId, "ClaimFilling");
            var response = await _httpClient.GetAsync(requestUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<SettingsWithEmployerStateDto>(responseContent);
            return new GenericResponse<SettingsWithEmployerStateDto>(null, employees);
        }

        public async Task<GenericResponse<SettingsWithEmployerStateDto>> UpdateClaimSetting(
            int employerId, UpdateClaimSettingDto updateClaimSettingDto)
        {
            var requestUrl = string.Format(EmployerEndpoints.UpdateEmployerClaimSetting, employerId);
            var requestContent = JsonContent.Create(updateClaimSettingDto);

            var response = await _httpClient.PutAsync(requestUrl, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<SettingsWithEmployerStateDto>(responseContent);
            return new GenericResponse<SettingsWithEmployerStateDto>(null, employees);
        }
    }
}
