using System.Net;
using System.Net.Http.Headers;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class BenefitsServiceClient : IBenefitsServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BenefitsServiceClient> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public BenefitsServiceClient(HttpClient httpClient, IJwtTokenService jwtTokenService, ILogger<BenefitsServiceClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            var userServiceApiUrl = configuration.GetValue<string>("BenefitsServiceApiUrl");
            if (string.IsNullOrEmpty(userServiceApiUrl))
            {
                _logger.LogError("BenefitsServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("BenefitsServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(userServiceApiUrl);
            _jwtTokenService = jwtTokenService;
        }

        public async Task<GenericHttpResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var token = _jwtTokenService.GetCachedToken();
            if (token == null) return new GenericHttpResponse<List<InsurancePackageDto>>(true, new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{BenefitsApiEndpoints.InsurancePackage}/{employerId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                 return new GenericHttpResponse<List<InsurancePackageDto>>(true, new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var packages = JsonConvert.DeserializeObject<List<InsurancePackageDto>>(responseContent);
            return new GenericHttpResponse<List<InsurancePackageDto>>(false, null, packages);
        }
    }

}
