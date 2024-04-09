using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
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

        public IActionResult Index()
        {
            return PartialView("_Enrollments");
        }

        [HttpGet("Data")]
        public async Task<IActionResult> GetEnrollmentsData(int employerId, int employeeId)
        {
            var viewModel = new EnrollmentsViewModel()
            {
                Enrollments = [],
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
                EmployerId = employerId,
                EmployeeId = employeeId
            };

            return Ok(viewModel);
        }
    }
}
