using CSI.IBTA.Customer.Endpoints;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;
using System.Text;

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
        
        public async Task<GenericResponse<bool?>> CreateEmployee(CreateEmployeeDto employee)
        {
            var jsonBody = JsonConvert.SerializeObject(employee);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(EmployeeEndpoints.CreateEmployee, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpError>(errorResponse) ?? HttpErrors.GenericError;
                var errorRes = new HttpError(error.Title, response.StatusCode);
                return new GenericResponse<bool?>(errorRes, null);
            }

            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<byte[]>> GetEncryptedEmployee(int employerId, int employeeId)
        {
            var requestUrl = string.Format(EmployeeEndpoints.EncryptedEmployee, employerId, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<byte[]>(responseContent);
            return new GenericResponse<byte[]>(null, employees);
        }
    }
}