using System.Net;
using System.Net.Http.Headers;
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
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthClient> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public UserServiceClient(HttpClient httpClient, IJwtTokenService jwtTokenService, ILogger<AuthClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            var userServiceApiUrl = configuration.GetValue<string>("UserServiceApiUrl");
            if (string.IsNullOrEmpty(userServiceApiUrl))
            {
                _logger.LogError("UserServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("AuthServiceApiUrl is missing in appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(userServiceApiUrl);
            _jwtTokenService = jwtTokenService;
        }

        public async Task<GenericResponse<IQueryable<EmployerDto>?>> GetEmployers()
        {
            var token = _jwtTokenService.GetCachedToken();
            if (token == null) return new GenericResponse<IQueryable<EmployerDto>?> (new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

        public async Task<GenericResponse<EmployerDto?>> CreateEmployer(CreateEmployerDto dto)
        {
            var token = _jwtTokenService.GetCachedToken();
            if (token == null) return new GenericResponse<EmployerDto?>( new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);

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

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            var token = _jwtTokenService.GetCachedToken();
            if (token == null) return new GenericResponse<EmployerDto?>(new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);

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

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return new GenericResponse<EmployerDto>(new HttpError("Invalid credentials", HttpStatusCode.Unauthorized), null);
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

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
