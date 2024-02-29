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
    }
}
