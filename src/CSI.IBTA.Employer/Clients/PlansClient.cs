using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Employer.Clients
{
    internal class PlansClient : IPlansClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public PlansClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<IEnumerable<PlanDto>>> GetPlansByNames(List<string> planNames)
        {
            var jsonBody = JsonConvert.SerializeObject(planNames);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(PlanEndpoints.ActivePlansByNames, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<HttpError>(responseContent) ?? HttpErrors.GenericError;
                var errorRes = new HttpError(error.Title, response.StatusCode);
                return new GenericResponse<IEnumerable<PlanDto>>(errorRes, null);
            }

            var plans = JsonConvert.DeserializeObject<IEnumerable<PlanDto>>(responseContent);
            return new GenericResponse<IEnumerable<PlanDto>>(null, plans);
        }
    }
}
