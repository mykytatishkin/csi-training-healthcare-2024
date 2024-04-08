using CSI.IBTA.Consumer.Endpoints;
using CSI.IBTA.Consumer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;

namespace CSI.IBTA.Consumer.Clients
{
    internal class EmployeesClient : IEmployeesClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EmployeesClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericResponse<PagedEmployeesResponse>> GetEmployees(
            int page,
            int pageSize,
            int employerId,
            string firstname = "",
            string lastname = "",
            string ssn = "")
        {
            var requestUrl = string.Format(EmployeeEndpoints.Employees, page, pageSize, employerId, firstname, lastname, ssn);
            var response = await _httpClient.GetAsync(requestUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<PagedEmployeesResponse>(responseContent);
            return new GenericResponse<PagedEmployeesResponse>(null, employees);
        }
    }
}
