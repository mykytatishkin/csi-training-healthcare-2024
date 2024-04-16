using CSI.IBTA.Customer.Endpoints;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Types;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;

namespace CSI.IBTA.Customer.Clients
{
    internal class ClaimsClient : IClaimsClient
    {
        private readonly AuthorizedHttpClient _httpClient;
        public ClaimsClient(AuthorizedHttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<GenericResponse<bool>> FileClaim(FileClaimDto dto)
        {
            var defaultErrorMessage = "Failed to file a claim";
            var formData = new MultipartFormDataContent()
            {
                { new StringContent(dto.Amount.ToString()), nameof(dto.Amount) },
                { new StringContent(dto.DateOfService.ToString()), nameof(dto.DateOfService) },
                { new StringContent(dto.EnrollmentId.ToString()), nameof(dto.EnrollmentId) },
               
            };

            using (var stream = new MemoryStream())
            {
                if (dto.Receipt != null)
                {
                    await dto.Receipt.CopyToAsync(stream);
                    stream.Position = 0;
                    formData.Add(new StreamContent(stream), nameof(dto.Receipt), dto.Receipt.FileName);
                }

                var response = await _httpClient.PutAsync(BenefitsServiceEndpoints.FileClaim, formData);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                    var errorMessage = error?.Title ?? response.ReasonPhrase ?? defaultErrorMessage;
                    return new GenericResponse<bool>(new HttpError(errorMessage, response.StatusCode), false);
                }

                return new GenericResponse<bool>(null, true);
            }
        }
    }
}
