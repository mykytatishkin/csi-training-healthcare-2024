using CSI.IBTA.Employer.Constants;
using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Models;
using CSI.IBTA.Shared.Constants;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("{controller}")]
    [TypeFilter(typeof(AuthenticationFilter))]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesClient _employeeClient;
        private readonly ISettingsClient _settingsClient;

        public EmployeesController(IEmployeesClient employeeClient, ISettingsClient settingsClient)
        {
            _employeeClient = employeeClient;
            _settingsClient = settingsClient;
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


            var addConsumersResponse = await _settingsClient.GetEmployerSetting(employerId, EmployerConstants.AddConsumers);

            if (addConsumersResponse.Error != null || addConsumersResponse.Result == null)
            {
                return Problem(title: "Failed to retrieve claim setting");
            }

            if (employeesResponse.Error != null || employeesResponse.Result == null)
            {
                return PartialView("_Employees");
            }

            var employees = employeesResponse.Result.Employees;

            var viewModel = new EmployeesSearchViewModel
            {
                Employees = employees,
                EmployerId = employerId,
                EmployerAddConsumers = addConsumersResponse.Result.State,
                Page = pageNumber ?? 1,
                TotalCount = employeesResponse.Result.TotalCount,
                TotalPages = employeesResponse.Result.TotalPages,
            };

            return PartialView("_Employees", viewModel);
        }

        [HttpGet("CreateEmployee")]
        public ActionResult CreateEmployee(int employerId, bool employerAddConsumers)
        {
            var viewModel = new EmployeeViewModel()
            {
                AllowAddConsumers = employerAddConsumers,
                Employee = new FullEmployeeDto(0, "", "", "", "", "", "", DateOnly.FromDateTime(DateTime.UtcNow), "", "", "", "", "", employerId),
            };
            return PartialView("_EmployeeForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EmployeeForm", viewModel);
            }



            var createEmployeeDto = new CreateEmployeeDto(
                viewModel.Employee.UserName,
                viewModel.Employee.Password,
                viewModel.Employee.FirstName,
                viewModel.Employee.LastName,
                viewModel.Employee.SSN,
                viewModel.Employee.PhoneNumber,
                viewModel.Employee.DateOfBirth,
                viewModel.Employee.Email,
                viewModel.Employee.AddressState,
                viewModel.Employee.AddressStreet,
                viewModel.Employee.AddressCity,
                viewModel.Employee.AddressZip,
                viewModel.Employee.EmployerId
            );

            var response = await _employeeClient.CreateEmployee(createEmployeeDto);

            if (response.Result == null)
            {
                return Problem(
                    statusCode: (int)response.Error?.StatusCode!,
                    title: response.Error.Title);
            }

            ModelState.Clear();
            viewModel.Employee = response.Result;
            ViewBag.SuccessMessage = "Employee created successfully!";
            return PartialView("_EmployeeForm", viewModel);
        }

        [HttpGet("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(int id, string employerId, bool employerAddConsumers)
        {
            var (httpError, employee) = await _employeeClient.GetEmployee(id);

            if (employee == null)
            {
                return Problem(
                    statusCode: (int)httpError!.StatusCode,
                    title: httpError.Title);
            }

            var viewModel = new EmployeeViewModel()
            {
                Employee = employee,
                AllowAddConsumers = employerAddConsumers
            };

            return PartialView("_EmployeeForm", viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel viewModel)
        {
            ModelState.Remove("Employee.UserName");
            ModelState.Remove("Employee.Password");

            if (!ModelState.IsValid)
            {
                return PartialView("_EmployeeForm", viewModel);
            }

            var updateEmployeeDto = new UpdateEmployeeDto(
                viewModel.Employee.Id,
                viewModel.Employee.Password,
                viewModel.Employee.FirstName,
                viewModel.Employee.LastName,
                viewModel.Employee.SSN,
                viewModel.Employee.PhoneNumber,
                viewModel.Employee.DateOfBirth,
                viewModel.Employee.Email,
                viewModel.Employee.AddressState,
                viewModel.Employee.AddressStreet,
                viewModel.Employee.AddressCity,
                viewModel.Employee.AddressZip
                );

            var response = await _employeeClient.UpdateEmployee(updateEmployeeDto);

            if (response.Error != null)
            {
                return Problem(
                    statusCode: (int)response.Error.StatusCode,
                    title: response.Error.Title);
            }

            ViewBag.SuccessMessage = "Employee updated successfully!";
            return PartialView("_EmployeeForm", viewModel);
        }
    }
}