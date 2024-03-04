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
        public async Task<GenericResponse<EmployerDto>> GetEmployerById(int id)
        {
            string requestUrl = string.Format(UserServiceApiEndpoints.Employer, id);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<EmployerDto>(HttpErrors.GenericError, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employer = JsonConvert.DeserializeObject<EmployerDto>(responseContent);

            return new GenericResponse<EmployerDto>(null, employer);
        }
    }
}
