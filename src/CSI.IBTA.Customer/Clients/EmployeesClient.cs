using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Endpoints;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Constants;
using CSI.IBTA.Shared.DTOs.Errors;

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

        public async Task<GenericResponse<byte[]>> GetEncryptedEmployee(int employerId, int employeeId)
        {
            var requestUrl = string.Format(UserServiceEndpoints.EncryptedEmployee, employerId, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var employees = await response.Content.ReadFromJsonAsync<byte[]>();
            return new GenericResponse<byte[]>(null, employees);
        }

        public async Task<GenericResponse<FullEmployeeDto>> GetEmployee(int employeeId)
        {
            var response = await _httpClient.GetAsync($"{UserServiceEndpoints.Employees}/{employeeId}");
            response.EnsureSuccessStatusCode();
            var employees = await response.Content.ReadFromJsonAsync<FullEmployeeDto>();
            return new GenericResponse<FullEmployeeDto>(null, employees);
        }

        public async Task<GenericResponse<string?>> GetEmployerLogo()
        {
            var response = await _httpClient.GetAsync(UserServiceEndpoints.EmployerLogo);
            response.EnsureSuccessStatusCode();
            var deserialized = await response.Content.ReadFromJsonAsync<EmployerLogoDto>();
            return new GenericResponse<string?>(null, deserialized?.EncodedLogo);
        }

        public async Task<GenericResponse<byte[]>> GetEncryptedEmployeeSettings(int employerId, int employeeId)
        {
            var requestUrl = string.Format(UserServiceEndpoints.EncryptedEmployeeSettings, employerId, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var encryptedData = await response.Content.ReadFromJsonAsync<byte[]>();
            return new GenericResponse<byte[]>(null, encryptedData);
        }

        public async Task<GenericResponse<bool>> GetEmployerClaimFillingSetting(int employeeId)
        {
            var requestUrl = string.Format(UserServiceEndpoints.GetClaimFillingSetting, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode) return new GenericResponse<bool>(HttpErrors.GenericError, false);
            var claimFillingSetting = await response.Content.ReadFromJsonAsync<bool>();
            return new GenericResponse<bool>(null, claimFillingSetting);
        }
    }
}