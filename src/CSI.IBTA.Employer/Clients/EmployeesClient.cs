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
            var requestUrl = string.Format(EmployeeEndpoints.Employees, page, pageSize, employerId, firstname, lastname, ssn);
            var response = await _httpClient.GetAsync(requestUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<PagedEmployeesResponse>(responseContent);
            return new GenericResponse<PagedEmployeesResponse>(null, employees);
        }

        public async Task<GenericResponse<FullEmployeeDto?>> CreateEmployee(CreateEmployeeDto employee)
        {
            var jsonBody = JsonConvert.SerializeObject(employee);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(EmployeeEndpoints.CreateEmployee, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<HttpError>(responseString) ?? HttpErrors.GenericError;
                var errorRes = new HttpError(error.Title, response.StatusCode);
                return new GenericResponse<FullEmployeeDto?>(errorRes, null);
            }
            var createdEmployee = JsonConvert.DeserializeObject<FullEmployeeDto>(responseString);
            return new GenericResponse<FullEmployeeDto?>(null, createdEmployee);
        }

        public async Task<GenericResponse<FullEmployeeDto>> GetEmployee(int id)
        {
            var response = await _httpClient.GetAsync(string.Format(EmployeeEndpoints.GetEmployee, id));

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpError>(errorResponse) ?? HttpErrors.GenericError;
                return new GenericResponse<FullEmployeeDto>(error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<FullEmployeeDto>(responseContent);
            return new GenericResponse<FullEmployeeDto>(null, employee);
        }

        public async Task<GenericResponse<bool?>> UpdateEmployee(UpdateEmployeeDto dto)
        {
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(string.Format(EmployeeEndpoints.UpdateEmployee, dto.Id), content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpError>(errorResponse) ?? HttpErrors.GenericError;
                return new GenericResponse<bool?>(error, false);
            }

            return new GenericResponse<bool?>(null, true);
        }
    }
}