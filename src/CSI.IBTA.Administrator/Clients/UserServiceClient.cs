using System.Net.Http.Headers;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.Entities;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServiceClient(HttpClient httpClient, IJwtTokenService jwtTokenService, ILogger<AuthClient> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClient;
            var userServiceApiUrl = configuration.GetValue<string>("UserServiceApiUrl");
            if (string.IsNullOrEmpty(userServiceApiUrl))
            {
                _logger.LogError("UserServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("AuthServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(userServiceApiUrl);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IQueryable<Employer>?> GetEmployers(string token)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(AuthApiEndpoints.UserServiceEmployers);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employers= JsonConvert.DeserializeObject<List<Employer>>(responseContent).AsQueryable();
            return employers;
        }
    }
}