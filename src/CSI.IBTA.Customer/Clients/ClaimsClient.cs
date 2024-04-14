﻿using CSI.IBTA.Customer.Endpoints;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Customer.Clients
{
    internal class ClaimsClient : IClaimsClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public ClaimsClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<PagedClaimsResponse>> GetClaimsByEmployee(int page, int pageSize, string employeeId, GetEnrollmentsDto dto)
        {
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var requestUrl = string.Format(BenefitsServiceEndpoints.ClaimsByEmployee, page, pageSize, employeeId);
            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var claims = JsonConvert.DeserializeObject<PagedClaimsResponse>(responseContent);
            return new GenericResponse<PagedClaimsResponse>(null, claims);
        }
    }
}
