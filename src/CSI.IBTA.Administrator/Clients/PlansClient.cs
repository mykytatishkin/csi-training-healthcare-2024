using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Administrator.Clients
{
    internal class PlansClient : IPlansClient
    {
        private readonly ILogger<PlansClient> _logger;
        private readonly AuthorizedHttpClient _httpClient;

        public PlansClient(AuthorizedHttpClient httpClient, ILogger<PlansClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
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

        public async Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes()
        {
            var response = await _httpClient.GetAsync(BenefitsServiceApiEndpoints.PlanTypes);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<IEnumerable<PlanTypeDto>>(null, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<IEnumerable<PlanTypeDto>>(responseContent);

            return new GenericResponse<IEnumerable<PlanTypeDto>>(null, plan);
        }


        public async Task<GenericResponse<bool?>> CreatePlan(CreatePlanDto planDto)
        {
            var jsonBody = JsonConvert.SerializeObject(planDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceApiEndpoints.Plans, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = HttpErrors.GenericError;

                return new GenericResponse<bool?>(error, null);
            }

            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<bool?>> UpdatePlan(int planId, UpdatePlanDto planDto)
        {
            var jsonBody = JsonConvert.SerializeObject(planDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(string.Format(BenefitsServiceApiEndpoints.Plan, planId), content);
            if (!response.IsSuccessStatusCode)
            {
                var error = HttpErrors.GenericError;

                return new GenericResponse<bool?>(error, null);
            }

            return new GenericResponse<bool?>(null, true);
        }
    }
}