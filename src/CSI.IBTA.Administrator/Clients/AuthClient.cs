using System.Net;
using System.Text;
using CSI.IBTA.Administrator.Constants;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs.Login;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSI.IBTA.Administrator.Clients
{
    internal class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthClient> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthClient(HttpClient httpClient, IJwtTokenService jwtTokenService, ILogger<AuthClient> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClient;
            var authServiceApiUrl = configuration.GetValue<string>("AuthServiceApiUrl");
            if (string.IsNullOrEmpty(authServiceApiUrl))
            {
                _logger.LogError("AuthServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("AuthServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(authServiceApiUrl);
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GenericResponse<bool>> Authenticate(LoginRequest request)
        {
            var defaultErrorMessage = "An error occurred during authentication";
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null");
                var error = new HttpError(defaultErrorMessage, HttpStatusCode.InternalServerError);
                return new GenericResponse<bool>(error, false);
            }

            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(AuthApiEndpoints.Auth, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return new GenericResponse<bool>(new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), false);

            if (!response.IsSuccessStatusCode)
                return new GenericResponse<bool>(new HttpError(response.ReasonPhrase ?? defaultErrorMessage, response.StatusCode), false);

            var responseContent = await response.Content.ReadAsStringAsync();
            var jToken = JsonConvert.DeserializeObject<JToken>(responseContent);

            if (jToken == null)
            {
                _logger.LogError("Failed to extract JToken from response");
                return new GenericResponse<bool>(new HttpError(defaultErrorMessage, response.StatusCode), false);
            }
            
            var token = jToken["token"]?.Value<string>();

            if (token == null)
            {
                _logger.LogError("Response did not have a token");
                return new GenericResponse<bool>(new HttpError(defaultErrorMessage, response.StatusCode), false);
            }
            
            bool isTokenValid = _jwtTokenService.IsTokenValid(token);
            
            if (!isTokenValid)
            {
                _logger.LogWarning("Tampered token was used to login");
                return new GenericResponse<bool>(new HttpError(defaultErrorMessage, response.StatusCode), false);
            }

            var isAdmin = _jwtTokenService.IsAdmin(token);

            if (!isAdmin)
                return new GenericResponse<bool>(new HttpError("Response to the portal is denied", response.StatusCode), false);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(TokenConstants.JwtTokenCookieName, token, _jwtTokenService.GetCookieOptions());
            return new GenericResponse<bool>(null, true);
        }
    }
}