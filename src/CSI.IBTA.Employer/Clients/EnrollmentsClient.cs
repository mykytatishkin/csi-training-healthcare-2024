using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
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
    }
}
