using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Employer.Clients
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
            var requestUrl = string.Format(UserServiceEndpoints.Employees, page, pageSize, employerId, firstname, lastname, ssn);
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
            var response = await _httpClient.PostAsync(UserServiceEndpoints.CreateEmployee, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpError>(errorResponse) ?? HttpErrors.GenericError;
                var errorRes = new HttpError(error.Title, response.StatusCode);
                return new GenericResponse<bool?>(errorRes, null);
            }

            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<IEnumerable<UserDto>>> GetEmployeesByUsernames(List<string> usernames, int employerId)
        {
            var jsonBody = JsonConvert.SerializeObject(usernames);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var requestUrl = string.Format(UserServiceEndpoints.UsersByUsernames, employerId);
            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<HttpError>(responseContent) ?? HttpErrors.GenericError;
                var errorRes = new HttpError(error.Title, response.StatusCode);
                return new GenericResponse<IEnumerable<UserDto>>(errorRes, null);
            }

            var users = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(responseContent);
            return new GenericResponse<IEnumerable<UserDto>>(null, users);
        }
        
        public async Task<GenericResponse<byte[]>> GetEncryptedEmployee(int employerId, int employeeId)
        {
            var requestUrl = string.Format(UserServiceEndpoints.EncryptedEmployee, employerId, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<byte[]>(responseContent);
            return new GenericResponse<byte[]>(null, employees);
        }
    }
}