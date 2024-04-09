using CSI.IBTA.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Employer.Models;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("[controller]")]
    public class EnrollmentsController : Controller
    {
        private readonly IInsuranceClient _insuranceClient;
        private readonly IEmployeesClient _employeesClient;

        public EnrollmentsController(IInsuranceClient enrollmentClient, IEmployeesClient employeesClient)
        {
            _insuranceClient = enrollmentClient;
            _employeesClient = employeesClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return PartialView("Enrollments/_Enrollments");
        }

        [HttpGet("Data")]
        public async Task<IActionResult> GetEnrollmentsData(int employerId, int employeeId)
        {
            var viewModel = new EnrollmentsViewModel()
            {
                Enrollments = [],
                Packages = []
            };

            var encryptedEmployeeResponse = await _employeesClient.GetEncryptedEmployee(employerId, employeeId);

            if (encryptedEmployeeResponse.Error != null)
            {
                return Problem(
                    detail: encryptedEmployeeResponse.Error.Title,
                    statusCode: (int)encryptedEmployeeResponse.Error.StatusCode
                );
            }

            var packagesResponse = await _insuranceClient.GetEmployerPackages(employerId);

            if (packagesResponse.Error != null)
            {
                return PartialView("Enrollments/_Enrollments", viewModel);
            }

            var plans = packagesResponse.Result!.SelectMany(p => p.Plans).ToList();
            
            var enrollmentsResponse = await _insuranceClient.GetEmployeeEnrollments(employeeId, new GetEnrollmentsDto(encryptedEmployeeResponse.Result!));

            if (enrollmentsResponse.Error != null)
            {
                return PartialView("Enrollments/_Enrollments", viewModel);
            }

            var fullEnrollmentDtos = enrollmentsResponse.Result!
                .Select(e => new FullEnrollmentDto(
                    e.Id,
                    plans.Where(p => p.Id == e.PlanId).First(),
                    e.Election,
                    e.Contribution,
                    e.EmployeeId));

            viewModel = new EnrollmentsViewModel()
            {
                Enrollments = fullEnrollmentDtos.ToList(),
                Packages = packagesResponse.Result!,
                EmployerId = employerId,
                EmployeeId = employeeId
            };

            return Ok(viewModel);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateEnrollments(int employerId, int employeeId, [FromBody] List<FullEnrollmentDto> enrollments)
        {
            var encryptedEmployeeResponse = await _employeesClient.GetEncryptedEmployee(employerId, employeeId);

            if (encryptedEmployeeResponse.Error != null)
            {
                return Problem(
                    detail: encryptedEmployeeResponse.Error.Title,
                    statusCode: (int)encryptedEmployeeResponse.Error.StatusCode
                );
            }

            var upsertDtos = enrollments.Select(e => new UpsertEnrollmentDto(e.Plan.Id, e.Election, e.Id)).ToList();
            var upsertDto = new UpsertEnrollmentsDto(upsertDtos, encryptedEmployeeResponse.Result!);

            var upsertResponse = await _insuranceClient.UpdateEnrollments(employerId, employeeId, upsertDto);

            if (upsertResponse.Error != null)
            {
                return Problem(
                    detail: upsertResponse.Error.Title,
                    statusCode: (int)upsertResponse.Error.StatusCode
                );
            }

            return Ok(upsertResponse.Result);
        }
    }
}
