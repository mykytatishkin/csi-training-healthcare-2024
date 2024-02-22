using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using CSI.IBTA.Administrator.Constants;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs.Login;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSI.IBTA.Administrator.Clients
{
    internal class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenService _jwtTokenService;

        public UserServiceClient(HttpClient httpClient, IJwtTokenService jwtTokenService, ILogger<AuthClient> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClient;
            _jwtTokenService = jwtTokenService;
            var userServiceApiUrl = configuration.GetValue<string>("UserServiceApiUrl");
            if (string.IsNullOrEmpty(userServiceApiUrl))
            {
                _logger.LogError("UserServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("AuthServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(userServiceApiUrl);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Employer>?> GetEmployers(string token)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return new GenericInternalResponse<UserDto>(true, InternalErrors.BaseInternalError, null);
            }

            string? token = _jwtTokenService.GetCachedToken();
            
            if (token == null)
            {
                return new GenericInternalResponse<UserDto>(true, InternalErrors.InvalidToken, null);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string requestUrl = string.Format(UserServiceApiEndpoints.User, userId);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericInternalResponse<UserDto>(true, InternalErrors.BaseInternalError, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(responseContent);

            return new GenericInternalResponse<UserDto>(false, null, user);
        }
    }
}