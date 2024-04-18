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
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
        }

        public async Task<GenericResponse<bool>> FileClaim(FileClaimDto dto)
        {
            var defaultErrorMessage = "Failed to file a claim";

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(dto.Amount.ToString()), nameof(dto.Amount));
                formData.Add(new StringContent(dto.DateOfService.ToString()), nameof(dto.DateOfService));
                formData.Add(new StringContent(dto.EnrollmentId.ToString()), nameof(dto.EnrollmentId));

                using (var settingsStream = new MemoryStream(dto.EncryptedEmployerEmployeeSettings))
                {
                    var encryptedSettingsContent = new StreamContent(settingsStream);
                    encryptedSettingsContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    formData.Add(encryptedSettingsContent, nameof(dto.EncryptedEmployerEmployeeSettings), "settings.bin");
                    
                    using (var stream = new MemoryStream())
                    {
                        await dto.Receipt.CopyToAsync(stream);
                        stream.Position = 0;
                        formData.Add(new StreamContent(stream), nameof(dto.Receipt), dto.Receipt.FileName);

                        var response = await _httpClient.PostAsync(BenefitsServiceEndpoints.FileClaim, formData);

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

        public async Task<GenericResponse<PagedClaimsResponse>> GetClaimsByEmployee(int page, int pageSize, int employeeId)
        {
            var requestUrl = string.Format(BenefitsServiceEndpoints.ClaimsByEmployee, page, pageSize, employeeId);
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var responseClaims = await response.Content.ReadFromJsonAsync<PagedClaimsResponse>();
            return new GenericResponse<PagedClaimsResponse>(null, responseClaims);
        }
        
    }
}
