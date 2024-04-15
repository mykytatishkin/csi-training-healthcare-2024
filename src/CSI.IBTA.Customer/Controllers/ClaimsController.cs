using CSI.IBTA.Customer.Constants;
using CSI.IBTA.Customer.Interfaces;
using CSI.IBTA.Customer.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
    [Route("[controller]")]
    public class ClaimsController : Controller
    {
        private readonly IClaimsClient _claimsClient;

        public ClaimsController(IClaimsClient claimsClient)
        {
            _claimsClient = claimsClient;
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

            var claims = claimsResponse.Result.Claims;

            var viewModel = new ClaimsSearchViewModel
            {
                Claims = claims,
                EmployeeId = employeeId,
                EmployerId = employerId,
                EmployerClaimFilling = true,
                Page = pageNumber ?? 1,
                TotalCount = claimsResponse.Result.TotalCount,
                TotalPages = claimsResponse.Result.TotalPages,
            };

            return PartialView("_Claims", viewModel);
        }
    }
}
