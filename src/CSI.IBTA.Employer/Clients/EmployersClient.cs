using CSI.IBTA.Employer.Endpoints;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Types;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;

namespace CSI.IBTA.Employer.Clients
{
    internal class EmployersClient : IEmployersClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EmployersClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto)
        {
            var defaultErrorMessage = "Failed to update employer";
            var formData = new MultipartFormDataContent()
            {
                { new StringContent(dto.Name), nameof(dto.Name) },
                { new StringContent(dto.Code), nameof(dto.Code) },
                { new StringContent(dto.Email), nameof(dto.Email) },
                { new StringContent(dto.Street), nameof(dto.Street) },
                { new StringContent(dto.City), nameof(dto.City) },
                { new StringContent(dto.State), nameof(dto.State) },
                { new StringContent(dto.ZipCode), nameof(dto.ZipCode) },
                { new StringContent(dto.Phone), nameof(dto.Phone) }
            };

            using (var stream = new MemoryStream())
            {
                if (dto.NewLogoFile != null)
                {
                    await dto.NewLogoFile.CopyToAsync(stream);
                    stream.Position = 0;
                    formData.Add(new StreamContent(stream), nameof(dto.NewLogoFile), dto.NewLogoFile.FileName);
                }

                var response = await _httpClient.PutAsync($"{UserServiceEndpoints.Employer}/{dto.Id}", formData);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                    var errorMessage = error?.Title ?? response.ReasonPhrase ?? defaultErrorMessage;
                    return new GenericResponse<EmployerDto?>(new HttpError(errorMessage, response.StatusCode), null);
                }

                var employerJson = await response.Content.ReadAsStringAsync();
                var employer = JsonConvert.DeserializeObject<EmployerDto>(employerJson);

                return new GenericResponse<EmployerDto?>(null, employer);
            }
        }

        public async Task<GenericResponse<EmployerDto>> GetEmployerById(int id)
        {
            var res = await _httpClient.GetAsync($"{UserServiceEndpoints.Employer}/{id}");
            res.EnsureSuccessStatusCode();
            var responseContent = await res.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<EmployerDto>(responseContent);
            return new GenericResponse<EmployerDto>(null, employees);
        }

        public async Task<GenericResponse<EmployerDto>> GetEmployerByAccountId(int id)
        {
            var res = await _httpClient.GetAsync($"{UserServiceEndpoints.GetEmployerByAccountId}/{id}");
            res.EnsureSuccessStatusCode();
            var responseContent = await res.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<EmployerDto>(responseContent);
            return new GenericResponse<EmployerDto>(null, employees);
        }
    }
}
