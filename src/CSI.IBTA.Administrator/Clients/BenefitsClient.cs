using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Administrator.Clients
{
    internal class BenefitsClient : IBenefitsClient
    {
        private readonly ILogger<BenefitsClient> _logger;
        private readonly AuthorizedHttpClient _httpClient;

        public BenefitsClient(AuthorizedHttpClient httpClient, ILogger<BenefitsClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericInternalResponse<PlanDto>> GetPlan(int id)
        {
            var response = await _httpClient.GetAsync(string.Format(BenefitsServiceApiEndpoints.Plan, id));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericInternalResponse<PlanDto>(true, null, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<PlanDto>(responseContent);

            return new GenericInternalResponse<PlanDto>(false, null, plan);
        }

        public async Task<GenericInternalResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes()
        {
            var response = await _httpClient.GetAsync(BenefitsServiceApiEndpoints.PlanTypes);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericInternalResponse<IEnumerable<PlanTypeDto>>(true, null, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<IEnumerable<PlanTypeDto>>(responseContent);

            return new GenericInternalResponse<IEnumerable<PlanTypeDto>>(false, null, plan);
        }


        public async Task<GenericInternalResponse<bool?>> CreatePlan(CreatePlanDto planDto)
        {
            var jsonBody = JsonConvert.SerializeObject(planDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceApiEndpoints.Plans, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = InternalErrors.GenericError;

                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }

        public async Task<GenericInternalResponse<bool?>> UpdatePlan(int planId, UpdatePlanDto planDto)
        {
            var jsonBody = JsonConvert.SerializeObject(planDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(string.Format(BenefitsServiceApiEndpoints.Plan, planId), content);
            if (!response.IsSuccessStatusCode)
            {
                var error = InternalErrors.GenericError;

                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }
    }
}