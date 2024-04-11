using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Endpoints;
using CSI.IBTA.Customer.Types;

namespace CSI.IBTA.Customer.Clients
{
    internal class EnrollmentsClient : IEnrollmentsClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EnrollmentsClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<PagedEnrollmentsResponse>> GetEmployeeEnrollmentsPaged(int employeeId, GetEnrollmentsDto dto, int pageNumber, int pageSize)
        {
            var requestUrl = string.Format(BenefitsServiceEndpoints.ActivePagedEnrollments, employeeId, pageNumber, pageSize);
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
                    return new GenericResponse<PagedEnrollmentsResponse>(error, null);
                }

                var genericError = new HttpError("Something went wrong...", response.StatusCode);
                return new GenericResponse<PagedEnrollmentsResponse>(genericError, null);
            }

            var enrollments = JsonConvert.DeserializeObject<PagedEnrollmentsResponse>(responseContent);
            return new GenericResponse<PagedEnrollmentsResponse>(null, enrollments);
        }
    }
}
