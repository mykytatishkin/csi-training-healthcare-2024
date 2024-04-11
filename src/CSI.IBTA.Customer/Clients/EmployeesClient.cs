using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Endpoints;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;

namespace CSI.IBTA.Customer.Clients
{
    internal class EmployeesClient : IEmployeesClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EmployeesClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericResponse<FullEmployeeDto>> GetEmployee(int employeeId)
        {
            var response = await _httpClient.GetAsync($"{UserServiceEndpoints.Employees}/{employeeId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<FullEmployeeDto>(responseContent);
            return new GenericResponse<FullEmployeeDto>(null, employees);
        }

        public async Task<GenericResponse<string?>> GetEmployerLogo()
        {
            var response = await _httpClient.GetAsync(UserServiceEndpoints.EmployerLogo);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var deserialized = JsonConvert.DeserializeObject<EmployerLogoDto>(responseContent);
            return new GenericResponse<string?>(null, deserialized?.EncodedLogo);
        }
    }
}