using CSI.IBTA.Administrator.Interfaces;
using System.Net.Http.Headers;

namespace CSI.IBTA.Administrator.Clients
{
    public class AuthorizedHttpClient : HttpClient
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthorizedHttpClient> _logger;

        public AuthorizedHttpClient(IJwtTokenService jwtTokenService, IConfiguration configuration, ILogger<AuthorizedHttpClient> logger)
        {
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _logger = logger;

            string token = _jwtTokenService.GetCachedToken() ?? string.Empty;
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void SetBaseAddress(string urlConfigurationString)
        {
            var apiUrl = _configuration.GetValue<string>("UserServiceApiUrl");

            if (string.IsNullOrEmpty(apiUrl))
            {
                _logger.LogError("API URL \"{urlConfigurationString}\" is missing in appsettings.json", urlConfigurationString);
                throw new InvalidOperationException($"API URL \"{urlConfigurationString}\" is missing in appsettings.json");
            }

            BaseAddress = new Uri(apiUrl);
        }
    }
}
