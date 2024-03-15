using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;
using CSI.IBTA.Administrator.Types;

namespace CSI.IBTA.Administrator.Clients
{
    public class ClaimsClient : IClaimsClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public ClaimsClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<ClaimDto>> GetClaim(int claimId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.Claims}/{claimId}");
            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<ClaimDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching claims", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var claim = JsonConvert.DeserializeObject<ClaimDto>(responseContent);
            return new GenericResponse<ClaimDto>(null, claim);
        }

        public async Task<GenericResponse<List<PlanDto>>> GetPlans(int? userId = null)
        {
            var response = await _httpClient.GetAsync(String.Format(BenefitsServiceApiEndpoints.Plan, "?customerId=" + userId));

            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<List<PlanDto>>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching plans", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plans = JsonConvert.DeserializeObject<List<PlanDto>>(responseContent);
            return new GenericResponse<List<PlanDto>>(null, plans);
        }

        public async Task<GenericResponse<PlanDto>> GetPlan(int planId)
        {
            var response = await _httpClient.GetAsync(String.Format(BenefitsServiceApiEndpoints.Plan, planId));

            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<PlanDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching plan", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<PlanDto>(responseContent);
            return new GenericResponse<PlanDto>(null, plan);
        }

        public async Task<GenericResponse<bool>> UpdateClaim(int claimId, UpdateClaimDto updateClaimDto)
        {
            //var jsonContent1 = JsonContent.Create(updateClaimDto.Claim);
            //var jsonContent = JsonContent.Create(updateClaimDto);
            var jsonBody = JsonConvert.SerializeObject(updateClaimDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{BenefitsServiceApiEndpoints.Claims}/{claimId}", content);
            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<bool>(new HttpError(response.ReasonPhrase ?? "Error occurred while updating claim", response.StatusCode), false);
            }

            return new GenericResponse<bool>(null, true);
        }


        public async Task<GenericResponse<IQueryable<ClaimDto>?>> GetClaims()
        {
            var response = await _httpClient.GetAsync(BenefitsServiceApiEndpoints.Claims);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<IQueryable<ClaimDto>?>(new HttpError(errorMessage, response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var claims = JsonConvert.DeserializeObject<List<ClaimDto>>(responseContent).AsQueryable();
            return new GenericResponse<IQueryable<ClaimDto>?>(null, claims);
        }

        public async Task<GenericResponse<ClaimDto?>> GetClaimDetails(int claimId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.Claims}/{claimId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<ClaimDto?>(new HttpError(errorMessage, response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var claim = JsonConvert.DeserializeObject<ClaimDto>(responseContent);
            return new GenericResponse<ClaimDto?>(null, claim);
        }

        public async Task<GenericResponse<bool>> ApproveClaim(int claimId)
        {
            var response = await _httpClient.PatchAsync($"{BenefitsServiceApiEndpoints.ApproveClaim}/{claimId}", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                var errorMessage = error?.title ?? response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<bool>(new HttpError(errorMessage, response.StatusCode), false);
            }

            return new GenericResponse<bool>(null, true);
        }

        public async Task<GenericResponse<bool>> DenyClaim(int claimId, DenyClaimDto dto)
        {
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{BenefitsServiceApiEndpoints.DenyClaim}/{claimId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                var errorMessage = error?.title ?? response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<bool>(new HttpError(errorMessage, response.StatusCode), false);
            }

            return new GenericResponse<bool>(null, true);
        }
    }
}
