using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
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
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var plans = JsonConvert.DeserializeObject<List<PlanDto>>(responseContent);
            return new GenericResponse<List<PlanDto>>(null, plans);
        }

        public async Task<GenericResponse<PlanDto>> GetPlan(int planId)
        {
            var response = await _httpClient.GetAsync(String.Format(BenefitsServiceApiEndpoints.Plan, planId));
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<PlanDto>(responseContent);
            return new GenericResponse<PlanDto>(null, plan);
        }

        public async Task<GenericResponse<List<PlanTypeDto>>> GetPlanTypes()
        {
            var response = await _httpClient.GetAsync(BenefitsServiceApiEndpoints.PlanTypes);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<List<PlanTypeDto>>(responseContent);

            return new GenericResponse<List<PlanTypeDto>>(null, plan);
        }


        public async Task<GenericResponse<bool?>> CreatePlan(CreatePlanDto planDto)
        {
            var jsonBody = JsonConvert.SerializeObject(planDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceApiEndpoints.Plans, content);
            response.EnsureSuccessStatusCode();
            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<bool?>> UpdatePlan(int planId, UpdatePlanDto planDto)
        {
            var jsonBody = JsonConvert.SerializeObject(planDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(string.Format(BenefitsServiceApiEndpoints.Plan, planId), content);
            response.EnsureSuccessStatusCode();
            return new GenericResponse<bool?>(null, true);
        }
    }
}