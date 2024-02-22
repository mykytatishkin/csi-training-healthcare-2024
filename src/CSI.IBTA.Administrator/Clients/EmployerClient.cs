using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net.Http.Headers;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class EmployerClient : IEmployerClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployerClient> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public EmployerClient(
            IHttpContextAccessor httpContextAccessor,
            ILogger<EmployerClient> logger,
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
        public async Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<EmployerDto>(true, InternalErrors.BaseInternalError, null);
            }

            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return new GenericInternalResponse<EmployerDto>(true, InternalErrors.InvalidToken, null);
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            string requestUrl = string.Format(UserServiceApiEndpoints.Employer, id);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.ReasonPhrase != null ?
                    new InternalError(response.ReasonPhrase) :
                    InternalErrors.BaseInternalError;
                return new GenericInternalResponse<EmployerDto>(true, error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employer = JsonConvert.DeserializeObject<EmployerDto>(responseContent);

            return new GenericInternalResponse<EmployerDto>(false, null, employer);
        }
    }
}
