using CSI.IBTA.Employer.Constants;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Models;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("{controller}")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesClient _employeeClient;

        public EmployeesController(IEmployeesClient employeeClient)
        {
            _employeeClient = employeeClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int employerId,
            string? firstnameFilter,
            string? lastnameFilter,
            string? ssnFilter,
            string? currentFirstnameFilter,
            string? currentLastnameFilter,
            string? currentSsnFilter,
            int? pageNumber)
        {
            if (firstnameFilter != null || lastnameFilter != null || ssnFilter != null)
            {
                pageNumber = 1;
            }

            firstnameFilter ??= currentFirstnameFilter;
            lastnameFilter ??= currentLastnameFilter;
            ssnFilter ??= currentSsnFilter;
            ViewData["CurrentFirstnameFilter"] = firstnameFilter;
            ViewData["CurrentLastnameFilter"] = lastnameFilter;
            ViewData["CurrentSsnFilter"] = ssnFilter;

            var employeesResponse = await _employeeClient.GetEmployees(
                pageNumber ?? 1,
                PaginationConstants.EmployeesPerPage,
                employerId,
                firstnameFilter ?? "",
                lastnameFilter ?? "",
                ssnFilter ?? "");

            if (employeesResponse.Error != null || employeesResponse.Result == null)
            {
                return PartialView("_Employees");
            }

            var employees = employeesResponse.Result.Employees;

            var viewModel = new EmployeesSearchViewModel
            {
                Employees = employees,
                EmployerId = employerId,
                Page = pageNumber ?? 1,
                TotalCount = employeesResponse.Result.TotalCount,
                TotalPages = employeesResponse.Result.TotalPages,
            };

            return PartialView("_Employees", viewModel);
        }

        [HttpGet("CreateEmployee")]
        public ActionResult CreateEmployee(int employerId)
        {
            var viewModel = new EmployeeViewModel()
            {
                ActionName = "CreateEmployee",
                EmployerId = employerId,
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            return PartialView("_EmployeeForm", viewModel);
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EmployeeForm", viewModel);
            }

            var createEmployeeDto = new CreateEmployeeDto(
                viewModel.Username,
                viewModel.Password,
                viewModel.Firstname,
                viewModel.Lastname,
                viewModel.SSN,
                viewModel.Phone,
                viewModel.DateOfBirth,
                viewModel.State,
                viewModel.Street,
                viewModel.City,
                viewModel.ZipCode,
                viewModel.EmployerId
            );

            var response = await _employeeClient.CreateEmployee(createEmployeeDto);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error.Title);
            }

            ViewBag.SuccessMessage = "Employee created successfully!";
            return PartialView("_EmployeeForm", new EmployeeViewModel());
        }
    }
}