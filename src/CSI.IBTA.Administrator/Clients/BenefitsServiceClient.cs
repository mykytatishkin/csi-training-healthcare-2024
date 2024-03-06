using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class BenefitsServiceClient : IBenefitsServiceClient
    {
        private readonly AuthorizedHttpClient _httpClient;
        private readonly ILogger<BenefitsServiceClient> _logger;

        public BenefitsServiceClient(AuthorizedHttpClient httpClient, ILogger<BenefitsServiceClient> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsApiEndpoints.InsurancePackage}/{employerId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                 return new GenericResponse<List<InsurancePackageDto>>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var packages = JsonConvert.DeserializeObject<List<InsurancePackageDto>>(responseContent);
            return new GenericResponse<List<InsurancePackageDto>>(null, packages);
        }

        public async Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId)
        {
            var response = await _httpClient.PatchAsync($"{BenefitsApiEndpoints.InsurancePackage}/{packageId}", null);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<InsurancePackageDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var package = JsonConvert.DeserializeObject<InsurancePackageDto>(responseContent);
            return new GenericResponse<InsurancePackageDto>(null, package);
        }

        public async Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId)
        { 
            var response = await _httpClient.DeleteAsync($"{BenefitsApiEndpoints.InsurancePackage}/{packageId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<bool>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), false);
            }

            return new GenericResponse<bool>(null, true);
        }
    }

}
