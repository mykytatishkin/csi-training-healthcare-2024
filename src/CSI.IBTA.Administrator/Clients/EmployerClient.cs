using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class EmployerClient : IEmployerClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EmployerClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }
        public async Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id)
        {
            string requestUrl = string.Format(UserServiceApiEndpoints.Employer, id);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = InternalErrors.GenericError;
                return new GenericInternalResponse<EmployerDto>(true, error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employer = JsonConvert.DeserializeObject<EmployerDto>(responseContent);

            return new GenericInternalResponse<EmployerDto>(false, null, employer);
        }
    }
}
