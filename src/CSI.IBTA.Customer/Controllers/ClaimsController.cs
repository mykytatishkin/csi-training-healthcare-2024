using CSI.IBTA.Customer.Constants;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
    [Route("[controller]")]
    public class ClaimsController : Controller
    {
        private readonly IEnrollmentsClient _enrollmentsClient;
        private readonly IEmployeesClient _employeesClient;
        private readonly IClaimsClient _claimsClient;
        public ClaimsController(IEnrollmentsClient enrollmentsClient, IEmployeesClient employeesClient, IClaimsClient claimsClient) 
        {
            _enrollmentsClient = enrollmentsClient;
            _employeesClient = employeesClient;
            _claimsClient = claimsClient;
        }

        [Route("FileClaim")]
        public async Task<IActionResult> FileClaimForm(int employerId, int employeeId, int enrollmentId)
        {
            var viewModel = new FileClaimViewModel()
            {
                EmployerId = employerId,
                EmployeeId = employeeId,
                Enrollments = [],
                Amount = 0,
                EnrollmentId = enrollmentId,
                DateOfService = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var encryptedEmployeeResponse = await _employeesClient.GetEncryptedEmployee(employerId, employeeId);
            if (encryptedEmployeeResponse.Error != null)
            {
                return Problem(
                    detail: encryptedEmployeeResponse.Error.Title,
                    statusCode: (int)encryptedEmployeeResponse.Error.StatusCode
                );
            }

            var enrollmentsResponse = await _enrollmentsClient.GetEmployeeEnrollmentsPaged(employeeId, new GetEnrollmentsDto(encryptedEmployeeResponse.Result!), 1, int.MaxValue);

            if (enrollmentsResponse.Error != null)
            {
                return PartialView("_FileClaim", viewModel);
            }

            viewModel.Enrollments = enrollmentsResponse.Result!.Enrollments;

            return PartialView("_FileClaim", viewModel);
        }

        [HttpPost]
        [Route("FileClaim")]
        public async Task<IActionResult> FileClaim(FileClaimViewModel model)
        {
            var encryptedResponse = await _employeesClient.GetEncryptedEmployeeSettings(model.EmployerId, model.EmployeeId);

            if (encryptedResponse.Result == null)
            {
                return Problem(
                    detail: encryptedResponse.Error?.Title,
                    statusCode: (int)encryptedResponse.Error?.StatusCode!
                );
            }

            var res = await _claimsClient.FileClaim(new FileClaimDto(model.DateOfService, model.EnrollmentId, model.Amount, model.Receipt, encryptedResponse.Result!));
            return Json(res);
        }

        public async Task<IActionResult> Index(
            int employeeId,
            int employerId,
            int? pageNumber)
        {

            var claimsResponse = await _claimsClient.GetClaimsByEmployee(pageNumber ?? 1,
                PaginationConstants.ClaimsPerPage, employeeId);

            if (claimsResponse.Error != null)
            {
                return Problem(
                    statusCode: (int)claimsResponse.Error.StatusCode,
                    title: claimsResponse.Error.Title);
            }

            var claims = claimsResponse.Result!.Claims;

            var settingsResponse = await _employeesClient.GetEmployerClaimFillingSetting(employeeId);

            var viewModel = new ClaimsSearchViewModel
            {
                Claims = claims,
                EmployeeId = employeeId,
                EmployerId = employerId,
                EmployerClaimFilling = settingsResponse?.Result ?? false,
                Page = pageNumber ?? 1,
                TotalCount = claimsResponse.Result.TotalCount,
                TotalPages = claimsResponse.Result.TotalPages,
            };

            return PartialView("_Claims", viewModel);
        }
    }
}
