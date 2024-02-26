using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class UserServiceClient : IUserServiceClient
    {
        private readonly ILogger<UserServiceClient> _logger;
        private readonly AuthorizedHttpClient _httpClient;

        public UserServiceClient(AuthorizedHttpClient httpClient, ILogger<UserServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<List<Employer>?> GetEmployers()
        {
            var response = await _httpClient.GetAsync(UserServiceApiEndpoints.Employers);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employersList = JsonConvert.DeserializeObject<List<Employer>>(responseContent);

            return employersList;
        }

        public async Task<GenericInternalResponse<UserDto>> GetUser(int userId)
        {
            string requestUrl = string.Format(UserServiceApiEndpoints.User, userId);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericInternalResponse<UserDto>(true, InternalErrors.GenericError, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(responseContent);

            return new GenericInternalResponse<UserDto>(false, null, user);
        }

        public async Task<IQueryable<SettingsDto>?> GetEmployerSettings(int employerId)
        {
            var token = _jwtTokenService.GetCachedToken();
            if (token == null) return null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(UserServiceApiEndpoints.Settings + "/" + employerId);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employersSettings = JsonConvert.DeserializeObject<List<SettingsDto>>(responseContent).AsQueryable();

            return employersSettings;
        }

        public async Task<IQueryable<SettingsDto>?> UpdateEmployerSettings(int employerId, List<SettingsDto>? SettingsDtos)
        {
            var token = _jwtTokenService.GetCachedToken();
            if (token == null) return null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = JsonContent.Create(SettingsDtos);
            var response = await _httpClient.PatchAsync(UserServiceApiEndpoints.Settings + "/" + employerId, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employersSettings = JsonConvert.DeserializeObject<List<SettingsDto>>(responseContent).AsQueryable();

            return employersSettings;
        }
    }
}