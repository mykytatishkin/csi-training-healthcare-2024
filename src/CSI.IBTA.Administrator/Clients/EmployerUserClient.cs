using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Net.Http;

namespace CSI.IBTA.Administrator.Clients
{
    internal class EmployerUserClient : IEmployerUserClient
    {
        private readonly AuthorizedHttpClient _httpClient;

        public EmployerUserClient(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("UserServiceApiUrl");
        }

        public async Task<GenericInternalResponse<List<UserDto>>> GetEmployerUsers(int employerId)
        {
            string requestUrl = string.Format(UserServiceApiEndpoints.EmployerUsers, employerId);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = InternalErrors.GenericError;
                return new GenericInternalResponse<List<UserDto>>(true, error, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var employerUsers = JsonConvert.DeserializeObject<List<UserDto>>(responseContent);

            return new GenericInternalResponse<List<UserDto>>(false, null, employerUsers);
        }

        public async Task<GenericInternalResponse<bool?>> CreateEmployerUser(CreateUserDto command)
        {
            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(UserServiceApiEndpoints.Users, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = InternalErrors.GenericError;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        error = new InternalError("User with this username already exists");
                        break;
                }

                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }

        public async Task<GenericInternalResponse<bool?>> UpdateEmployerUser(PutUserDto command, int userId)
        {
            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            string requestUrl = string.Format(UserServiceApiEndpoints.EmployerUser, userId);
            var response = await _httpClient.PutAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = InternalErrors.GenericError;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        error = new InternalError("User with this username already exists");
                        break;
                }

                return new GenericInternalResponse<bool?>(true, error, null);
            }

            return new GenericInternalResponse<bool?>(false, null, true);
        }
    }
}
