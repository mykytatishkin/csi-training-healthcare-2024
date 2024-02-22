using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net.Http.Headers;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Administrator.Clients
{
    internal class EmployerUserClient : IEmployerUserClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployerUserClient> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public EmployerUserClient(
            IHttpContextAccessor httpContextAccessor,
            ILogger<EmployerUserClient> logger,
            HttpClient httpClient,
            IConfiguration configuration,
            IJwtTokenService jwtTokenService)
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
            _jwtTokenService = jwtTokenService;
        }

        public async Task<GenericInternalResponse<List<UserDto>>> GetEmployerUsers(int employerId)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<List<UserDto>>(true, InternalErrors.BaseInternalError, null);
            }

            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return new GenericInternalResponse<List<UserDto>>(true, InternalErrors.InvalidToken, null);
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            string requestUrl = string.Format(UserServiceApiEndpoints.EmployerUsers, employerId);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.ReasonPhrase != null ?
                    new InternalError(response.ReasonPhrase) :
                    InternalErrors.BaseInternalError;
                return new GenericInternalResponse<List<UserDto>>(true, error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employerUsers = JsonConvert.DeserializeObject<List<UserDto>>(responseContent);

            return new GenericInternalResponse<List<UserDto>>(false, null, employerUsers);
        }

        public async Task<GenericInternalResponse<bool?>> CreateEmployerUser(CreateUserDto command)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<bool?>(true, InternalErrors.BaseInternalError, null);
            }

            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return new GenericInternalResponse<bool?>(true, InternalErrors.InvalidToken, null);
            }

            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync(UserServiceApiEndpoints.Users, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.ReasonPhrase != null ?
                    new InternalError(response.ReasonPhrase) :
                    InternalErrors.BaseInternalError;
                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }

        public async Task<GenericInternalResponse<bool?>> UpdateEmployerUser(PutUserDto command, int userId)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<bool?>(true, InternalErrors.BaseInternalError, null);
            }

            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return new GenericInternalResponse<bool?>(true, InternalErrors.InvalidToken, null);
            }

            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            string requestUrl = string.Format(UserServiceApiEndpoints.EmployerUser, userId);
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
