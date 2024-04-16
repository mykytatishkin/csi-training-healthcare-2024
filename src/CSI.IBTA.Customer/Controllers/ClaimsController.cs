using CSI.IBTA.Customer.Clients;
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

        public async Task<IActionResult> Index(int employerId, int employeeId)
        {
            var viewModel = new FileClaimViewModel()
            {
                Enrollments = [],
                Amount = 0,
                EnrollmentId = 0,
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
                return PartialView("_Claims", viewModel);
            }


            viewModel = new FileClaimViewModel()
            {
                Enrollments = enrollmentsResponse.Result!.Enrollments,
            };
            

            return PartialView("_Claims", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FileClaim(FileClaimViewModel model)
        {
            var e = model.Enrollments;
            var res = await _claimsClient.FileClaim(new FileClaimDto(model.DateOfService, model.EnrollmentId, model.Amount, model.Receipt));
            return Json(res);
        }
    }
}
