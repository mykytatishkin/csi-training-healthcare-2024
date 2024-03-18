﻿using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Types;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<GenericResponse<PagedClaimsResponse>> GetClaims(int page, int pageSize, string claimNumber = "", string employerId = "", string claimStatus = "")
        {
            var requestUrl = string.Format(BenefitsServiceApiEndpoints.ClaimsList, page, pageSize, claimNumber, employerId, claimStatus);

            var response = await _httpClient.GetAsync(requestUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var responseError = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                if (responseError?.title != null)
                {
                    var error = new HttpError(responseError.title, response.StatusCode);
                    return new GenericResponse<PagedClaimsResponse>(error, null);
                }

                var defaultError = HttpErrors.GenericError;
                return new GenericResponse<PagedClaimsResponse>(defaultError, null);
            }

            var claims = JsonConvert.DeserializeObject<PagedClaimsResponse>(responseContent);
            return new GenericResponse<PagedClaimsResponse>(null, claims);
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
