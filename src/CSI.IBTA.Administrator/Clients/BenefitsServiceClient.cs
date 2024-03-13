using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<GenericResponse<ClaimDto>> GetClaim(int claimId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.Claims}/{claimId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<ClaimDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching claims", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var claim = JsonConvert.DeserializeObject<ClaimDto>(responseContent);
            return new GenericResponse<ClaimDto>(null, claim);
        }

        public async Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{employerId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                 return new GenericResponse<List<InsurancePackageDto>>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var packages = JsonConvert.DeserializeObject<List<InsurancePackageDto>>(responseContent);
            return new GenericResponse<List<InsurancePackageDto>>(null, packages);
        }

        public async Task<GenericResponse<List<PlanIdAndNameDto>>> GetPlans()
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.Plan}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<List<PlanIdAndNameDto>>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching plans", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plans = JsonConvert.DeserializeObject<List<PlanIdAndNameDto>>(responseContent);
            return new GenericResponse<List<PlanIdAndNameDto>>(null, plans);
        }

        public async Task<GenericResponse<PlanIdAndNameDto>> GetPlan(int planId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.Plan}/{planId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<PlanIdAndNameDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching plan", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<PlanIdAndNameDto>(responseContent);
            return new GenericResponse<PlanIdAndNameDto>(null, plan);
        }

        public async Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId)
        {
            var response = await _httpClient.PatchAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{packageId}", null);

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
            var response = await _httpClient.DeleteAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{packageId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<bool>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), false);
            }

            return new GenericResponse<bool>(null, true);
        }

        public async Task<GenericResponse<ClaimDto>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto)
        {
            var jsonBody = JsonConvert.SerializeObject(updateClaimDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{BenefitsServiceApiEndpoints.Claims}/{claimId}", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<ClaimDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while updating claim", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var claim = JsonConvert.DeserializeObject<ClaimDto>(responseContent);
            return new GenericResponse<ClaimDto>(null, claim);
        }
    }

}
