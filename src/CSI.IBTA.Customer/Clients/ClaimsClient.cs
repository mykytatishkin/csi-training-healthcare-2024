using CSI.IBTA.Customer.Endpoints;
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

        public async Task<GenericResponse<PagedClaimsResponse>> GetClaimsByEmployee(int page, int pageSize, int employeeId)
        {

            var requestUrl = string.Format(BenefitsServiceEndpoints.ClaimsByEmployee, page, pageSize, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var responseClaims = await response.Content.ReadFromJsonAsync<PagedClaimsResponse>();
            return new GenericResponse<PagedClaimsResponse>(null, responseClaims);
        }
    }
}
