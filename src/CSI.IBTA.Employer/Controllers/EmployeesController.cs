using CSI.IBTA.Employer.Constants;
using CSI.IBTA.Employer.Filters;
using CSI.IBTA.Employer.Interfaces;
using CSI.IBTA.Employer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Employer.Controllers
{
    [Route("{controller}")]
    [TypeFilter(typeof(AuthenticationFilter))]
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
    }
}
