using System.Text;
using CSI.IBTA.Administrator.Interfaces;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthClient> _logger;

        public AuthClient(ILogger<AuthClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            var authServiceApiUrl = configuration.GetValue<string>("AuthServiceApiUrl");
            if (string.IsNullOrEmpty(authServiceApiUrl))
            {
                _logger.LogError("AuthServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("AuthServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(authServiceApiUrl);
        }

        public async Task<HttpResponseMessage> PostAsync <T> (T dto, string apiEndpoint) where T : class
        {
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(apiEndpoint, content);
        }
    }
}