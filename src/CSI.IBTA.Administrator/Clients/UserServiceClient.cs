using System.Net;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Types;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using Newtonsoft.Json;

namespace CSI.IBTA.Administrator.Clients
{
    internal class UserServiceClient : IUserServiceClient
    {
        private readonly ILogger<UserServiceClient> _logger;
        private readonly AuthorizedHttpClient _httpClient;

        public UserServiceClient(AuthorizedHttpClient httpClient, ILogger<UserServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericResponse<IQueryable<EmployerDto>?>> GetEmployers()
        {

            var response = await _httpClient.GetAsync(UserApiEndpoints.Employer);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                var errorMessage = response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<IQueryable<EmployerDto>?>(new HttpError(errorMessage, response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employers = JsonConvert.DeserializeObject<List<EmployerDto>>(responseContent).AsQueryable();
            return new GenericResponse<IQueryable<EmployerDto>?>(null, employers);
        }

        public async Task<GenericResponse<UserDto>> GetUser(int userId)
        {
            string requestUrl = string.Format(UserServiceApiEndpoints.User, userId);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<UserDto>(HttpErrors.GenericError, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(responseContent);

            return new GenericResponse<UserDto>(null, user);
        }

        public async Task<GenericResponse<IQueryable<SettingsDto>?>> GetEmployerSettings(int employerId)
        {
            string requestUrl = string.Format(UserServiceApiEndpoints.Settings, employerId);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<IQueryable<SettingsDto>?>(new HttpError(response.ReasonPhrase, response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employersSettings = JsonConvert.DeserializeObject<List<SettingsDto>>(responseContent).AsQueryable();

            return new GenericResponse<IQueryable<SettingsDto>?>(null, employersSettings);
        }

        public async Task<GenericResponse<IQueryable<SettingsDto>?>> UpdateEmployerSettings(int employerId, List<SettingsDto>? SettingsDtos)
        {
            var content = JsonContent.Create(SettingsDtos);
            string requestUrl = string.Format(UserServiceApiEndpoints.Settings, employerId);
            var response = await _httpClient.PatchAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                var errorMessage = response.ReasonPhrase ?? "Something went wrong";
                return new GenericResponse<IQueryable<SettingsDto>?>(new HttpError(errorMessage, response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employersSettings = JsonConvert.DeserializeObject<List<SettingsDto>>(responseContent).AsQueryable();

            return new GenericResponse<IQueryable<SettingsDto>?>(null, employersSettings);
        }

        public async Task<GenericResponse<EmployerDto?>> CreateEmployer(CreateEmployerDto dto)
        {
            var defaultErrorMessage = "Failed to create a new employer";
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
                if (dto.LogoFile != null)
                {
                    await dto.LogoFile.CopyToAsync(stream);
                    stream.Position = 0;
                    formData.Add(new StreamContent(stream), nameof(dto.LogoFile), dto.LogoFile.FileName);
                }

                var response = await _httpClient.PostAsync(UserApiEndpoints.Employer, formData);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return new GenericResponse<EmployerDto?>(new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                    var errorMessage = error?.title ?? response.ReasonPhrase ?? defaultErrorMessage;
                    return new GenericResponse<EmployerDto?>(new HttpError(errorMessage, response.StatusCode), null);
                }

                var employerJson = await response.Content.ReadAsStringAsync();
                var employer = JsonConvert.DeserializeObject<EmployerDto>(employerJson);

                return new GenericResponse<EmployerDto?>(null, employer);
            }
        }

        public async Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId)
        {
            var defaultErrorMessage = "Failed to create a new employer";
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

                var response = await _httpClient.PutAsync($"{UserApiEndpoints.Employer}/{employerId}", formData);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return new GenericResponse<EmployerDto?>(new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(errorJson);
                    var errorMessage = error?.title ?? response.ReasonPhrase ?? defaultErrorMessage;
                    return new GenericResponse<EmployerDto?>(new HttpError(errorMessage, response.StatusCode), null);
                }

                var employerJson = await response.Content.ReadAsStringAsync();
                var employer = JsonConvert.DeserializeObject<EmployerDto>(employerJson);

                return new GenericResponse<EmployerDto?>(null, employer);
            }
        }

        public async Task<GenericResponse<EmployerDto>> GetEmployerById(int id)
        {
            string requestUrl = $"{UserApiEndpoints.Employer}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = new HttpError(response.ReasonPhrase ?? "Something went wrong", response.StatusCode);
                return new GenericResponse<EmployerDto>(error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employer = JsonConvert.DeserializeObject<EmployerDto>(responseContent);

            return new GenericResponse<EmployerDto>(null, employer);
        }
    }

}
