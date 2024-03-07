using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;

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

        public async Task<GenericResponse<IQueryable<ClaimDto>?>> GetClaims()
        {
            var response = await _httpClient.GetAsync(BenefitsServiceApiEndpoints.Claims);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<IQueryable<ClaimDto>?>(new HttpError(errorMessage, response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employers = JsonConvert.DeserializeObject<List<ClaimDto>>(responseContent).AsQueryable();
            return new GenericResponse<IQueryable<ClaimDto>?>(null, employers);
        }
    }
}
