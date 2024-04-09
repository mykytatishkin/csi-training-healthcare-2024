using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Employer.Types;
using Newtonsoft.Json;
using System.Text;

namespace CSI.IBTA.Employer.Clients
{
    internal class EnrollmentsClient : IEnrollmentsClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EnrollmentsClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<IEnumerable<EnrollmentDto>>> GetEnrollmentsByUserIds(List<int> userIds)
        {
            var jsonBody = JsonConvert.SerializeObject(userIds);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceEndpoints.EnrollmentsByUserIds, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<HttpError>(responseContent) ?? HttpErrors.GenericError;
                var errorRes = new HttpError(error.Title, response.StatusCode);
                return new GenericResponse<IEnumerable<EnrollmentDto>>(errorRes, null);
            }

            var enrollments = JsonConvert.DeserializeObject<IEnumerable<EnrollmentDto>>(responseContent);
            return new GenericResponse<IEnumerable<EnrollmentDto>>(null, enrollments);
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> GetEmployeeEnrollments(int employeeId, GetEnrollmentsDto dto)
        {
            var requestUrl = string.Format(BenefitsServiceEndpoints.Enrollments, employeeId);
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var responseError = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                if (responseError?.Title != null)
                {
                    var error = new HttpError(responseError.Title, response.StatusCode);
                    return new GenericResponse<List<EnrollmentDto>>(error, null);
                }

                var genericError = new HttpError("Something went wrong...", response.StatusCode);
                return new GenericResponse<List<EnrollmentDto>>(genericError, null);
            }

            var enrollments = JsonConvert.DeserializeObject<List<EnrollmentDto>>(responseContent);
            return new GenericResponse<List<EnrollmentDto>>(null, enrollments);
        }

        public async Task<GenericResponse<List<FullInsurancePackageDto>>> GetEmployerPackages(int employerId)
        {
            var response = await _httpClient.GetAsync(BenefitsServiceEndpoints.InsurancePackagesByEmployer);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var responseError = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                if (responseError?.Title != null)
                {
                    var error = new HttpError(responseError.Title, response.StatusCode);
                    return new GenericResponse<List<FullInsurancePackageDto>>(error, null);
                }

                var genericError = new HttpError("Something went wrong...", response.StatusCode);
                return new GenericResponse<List<FullInsurancePackageDto>>(genericError, null);
            }

            var plans = JsonConvert.DeserializeObject<List<FullInsurancePackageDto>>(responseContent);
            return new GenericResponse<List<FullInsurancePackageDto>>(null, plans);
        }

        public async Task<GenericResponse<List<EnrollmentDto>>> UpdateEnrollments(int employerId, int employeeId, UpsertEnrollmentsDto updateEnrollmentsDto)
        {
            var requestUrl = string.Format(BenefitsServiceEndpoints.UpdateEnrollments, employerId, employeeId);
            var jsonBody = JsonConvert.SerializeObject(updateEnrollmentsDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var responseError = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                if (responseError?.Title != null)
                {
                    var error = new HttpError(responseError.Title, response.StatusCode);
                    return new GenericResponse<List<EnrollmentDto>>(error, null);
                }

                var genericError = new HttpError("Something went wrong...", response.StatusCode);
                return new GenericResponse<List<EnrollmentDto>>(genericError, null);
            }

            var enrollments = JsonConvert.DeserializeObject<List<EnrollmentDto>>(responseContent);
            return new GenericResponse<List<EnrollmentDto>>(null, enrollments);
        }
    }
}
