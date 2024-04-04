﻿using CSI.IBTA.Employer.Endpoints;
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

        public async Task<GenericResponse<CreateEmployeeDto>> GetEmployee(int id)
        {
            var response = await _httpClient.GetAsync(string.Format(EmployeeEndpoints.GetEmployee, id));

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpError>(errorResponse) ?? HttpErrors.GenericError;
                return new GenericResponse<CreateEmployeeDto>(error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<CreateEmployeeDto>(responseContent);
            return new GenericResponse<CreateEmployeeDto>(null, employee);
        }

        public async Task<GenericResponse<bool?>> UpdateEmployee(int? id, CreateEmployeeDto dto)
        {
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(string.Format(EmployeeEndpoints.UpdateEmployee, id), content);

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