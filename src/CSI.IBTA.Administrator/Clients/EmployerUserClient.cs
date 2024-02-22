using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace CSI.IBTA.Administrator.Clients
{
    internal class EmployerUserClient : IEmployerUserClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployerUserClient> _logger;

        public EmployerUserClient(
            IHttpContextAccessor httpContextAccessor,
            ILogger<EmployerUserClient> logger,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _httpClient = httpClient;

            var userServiceApiUrl = configuration.GetValue<string>("UserServiceApiUrl");
            if (string.IsNullOrEmpty(userServiceApiUrl))
            {
                _logger.LogError("UserServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("UserServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(userServiceApiUrl);
        }

        public async Task<GenericInternalResponse<bool?>> CreateEmployerUser(CreateUserDto command, string token)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<bool?>(true, InternalErrors.BaseInternalError, null);
            }

            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync(UserServiceApiEndpoints.CreateUser, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.ReasonPhrase != null ?
                    new InternalError(response.ReasonPhrase) :
                    InternalErrors.BaseInternalError;
                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }

        public async Task<GenericInternalResponse<bool?>> UpdateEmployerUser(PutUserDto command, int userId, string token)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<bool?>(true, InternalErrors.BaseInternalError, null);
            }

            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            string requestUrl = string.Format(UserServiceApiEndpoints.PatchEmployerUser, userId);
            var response = await _httpClient.PutAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.ReasonPhrase != null ?
                    new InternalError(response.ReasonPhrase) :
                    InternalErrors.BaseInternalError;
                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }
    }
}
