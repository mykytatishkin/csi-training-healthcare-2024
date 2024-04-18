using CSI.IBTA.Customer.Constants;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
    [Route("[controller]")]
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentsClient _enrollmentClient;
        private readonly IEmployeesClient _employeesClient;

        public EnrollmentsController(IEnrollmentsClient enrollmentClient, IEmployeesClient employeesClient)
        {
            _enrollmentClient = enrollmentClient;
            _employeesClient = employeesClient;
        }
        
        public async Task<IActionResult> Index(int employerId, int employeeId, int? pageNumber)
        {
            var viewModel = new EnrollmentsViewModel()
            {
                Enrollments = [],
                Page = 1,
                TotalCount = 0,
                TotalPages = 0,
                EmployeeId = employeeId,
                EmployerId = employerId,
                AllowClaimFilling = false,
            };

            var encryptedEmployeeResponse = await _employeesClient.GetEncryptedEmployee(employerId, employeeId);

            if (encryptedEmployeeResponse.Error != null)
            {
                return Problem(
                    detail: encryptedEmployeeResponse.Error.Title,
                    statusCode: (int)encryptedEmployeeResponse.Error.StatusCode
                );
            }

            var enrollmentsResponse = await _enrollmentClient.GetEmployeeEnrollmentsPaged(
                employeeId, 
                new GetEnrollmentsDto(encryptedEmployeeResponse.Result!), 
                pageNumber ?? 1,
                PaginationConstants.EnrollmentsPerPage);

            if (enrollmentsResponse.Error != null)
            {
                return PartialView("_Enrollments", viewModel);
            }

            var settingsResponse = await _employeesClient.GetEmployerClaimFillingSetting(employeeId);

            viewModel = new EnrollmentsViewModel()
            {
                Enrollments = enrollmentsResponse.Result!.Enrollments,
                TotalCount = enrollmentsResponse.Result.TotalCount,
                Page = enrollmentsResponse.Result.CurrentPage,
                TotalPages = enrollmentsResponse.Result.TotalPages,
                EmployerId = employerId,
                EmployeeId = employeeId,
                AllowClaimFilling = settingsResponse.Result
            };

            return PartialView("_Enrollments", viewModel);
        }
    }
}
