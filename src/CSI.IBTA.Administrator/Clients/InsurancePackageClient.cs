using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace CSI.IBTA.Administrator.Clients
{
    internal class InsurancePackageClient : IInsurancePackageClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public InsurancePackageClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command)
        {
            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceApiEndpoints.InsurancePackages, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = HttpErrors.GenericError;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        error = new HttpError("Insurance package with this name already exists", HttpStatusCode.Conflict);
                        break;
                }

                return new GenericResponse<bool?>(error, null);
            }

            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<IEnumerable<PlanTypeDto>>> GetPlanTypes()
        {
            var response = await _httpClient.GetAsync(BenefitsServiceApiEndpoints.PlanTypes);
            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<IEnumerable<PlanTypeDto>>(null, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var plan = JsonConvert.DeserializeObject<IEnumerable<PlanTypeDto>>(responseContent);

            return new GenericResponse<IEnumerable<PlanTypeDto>>(null, plan);
        }

        public async Task<GenericResponse<int?>> GetInsurancePackage(int insurancePackageId)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResponse<bool?>> UpdateInsurancePackage(CreateInsurancePackageDto command)
        {
            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceApiEndpoints.InsurancePackages, content);

            return new GenericResponse<bool?>(null, true);
        }
    }
}