using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;

namespace CSI.IBTA.Employer.Clients
{
    internal class EmployersClient : IEmployersClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EmployersClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericResponse<EmployerDto>> GetEmployerById(int id)
        {
            var res = await _httpClient.GetAsync($"{EmployerEndpoints.Employer}/{id}");
            res.EnsureSuccessStatusCode();
            var responseContent = await res.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<EmployerDto>(responseContent);
            return new GenericResponse<EmployerDto>(null, employees);
        }

        public Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto)
        {
            throw new NotImplementedException();
        }
    }
}