﻿using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Endpoints;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using CSI.IBTA.Administrator.Types;

namespace CSI.IBTA.Administrator.Clients
{
    internal class InsurancePackageClient : IInsurancePackageClient
    {
        private readonly AuthorizedHttpClient _httpClient;
        private readonly ILogger<InsurancePackageClient> _logger;

        public InsurancePackageClient(AuthorizedHttpClient httpClient, ILogger<InsurancePackageClient> logger)
        {
            _httpClient = httpClient;
            _httpClient.SetBaseAddress("BenefitsServiceApiUrl");
            _logger = logger;
        }

        public async Task<GenericResponse<bool?>> CreateInsurancePackage(CreateInsurancePackageDto command)
        {
            var jsonBody = JsonConvert.SerializeObject(command);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BenefitsServiceApiEndpoints.InsurancePackages, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = HttpErrors.GenericError;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        var errorMessage = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
                        error = new HttpError(errorMessage?.title ?? "Something went wrong", HttpStatusCode.Conflict);
                        break;
                }

                return new GenericResponse<bool?>(error, null);
            }

            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<FullInsurancePackageDto>> GetInsurancePackage(int insurancePackageId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{insurancePackageId}");
            if (!response.IsSuccessStatusCode)
            {
                return new GenericResponse<FullInsurancePackageDto>(HttpErrors.GenericError, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var package = JsonConvert.DeserializeObject<FullInsurancePackageDto>(responseContent);

            return new GenericResponse<FullInsurancePackageDto>(null, package);
        }

        public async Task<GenericResponse<bool?>> UpdateInsurancePackage(UpdateInsurancePackageDto dto)
        {
            var jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var res = await _httpClient.PutAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{dto.Id}", content);

            if (!res.IsSuccessStatusCode)
            {
                var error = HttpErrors.GenericError;

                switch (res.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        var errorMessage = JsonConvert.DeserializeObject<ErrorResponse>(await res.Content.ReadAsStringAsync());
                        error = new HttpError(errorMessage?.title ?? "Something went wrong", HttpStatusCode.Conflict);
                        break;
                }

                return new GenericResponse<bool?>(error, null);
            }
            return new GenericResponse<bool?>(null, true);
        }

        public async Task<GenericResponse<List<InsurancePackageDto>>> GetInsurancePackages(int employerId)
        {
            var response = await _httpClient.GetAsync($"{BenefitsServiceApiEndpoints.InsurancePackagesByEmployer}/{employerId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<List<InsurancePackageDto>>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var packages = JsonConvert.DeserializeObject<List<InsurancePackageDto>>(responseContent);
            return new GenericResponse<List<InsurancePackageDto>>(null, packages);
        }

        public async Task<GenericResponse<InsurancePackageDto>> InitializeInsurancePackage(int packageId)
        {
            var response = await _httpClient.PatchAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{packageId}", null);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<InsurancePackageDto>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var package = JsonConvert.DeserializeObject<InsurancePackageDto>(responseContent);
            return new GenericResponse<InsurancePackageDto>(null, package);
        }

        public async Task<GenericResponse<bool>> RemoveInsurancePackage(int packageId)
        {
            var response = await _httpClient.DeleteAsync($"{BenefitsServiceApiEndpoints.InsurancePackages}/{packageId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request unsuccessful");
                return new GenericResponse<bool>(new HttpError(response.ReasonPhrase ?? "Error occurred while fetching insurance packages", response.StatusCode), false);
            }

            return new GenericResponse<bool>(null, true);
        }
    }
}