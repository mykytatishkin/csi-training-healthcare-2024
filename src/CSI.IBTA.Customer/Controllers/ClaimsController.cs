using CSI.IBTA.Customer.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Customer.Controllers
{
    [Route("[controller]")]
    public class ClaimsController : Controller
    {
        public IActionResult Index(
            int employerId,
            string? firstnameFilter,
            string? lastnameFilter,
            string? ssnFilter,
            string? currentFirstnameFilter,
            string? currentLastnameFilter,
            string? currentSsnFilter,
            int? pageNumber)
        {
            var claims = new List<ClaimDto>();
            claims.Add(new ClaimDto(1, 3, 4, 5, "LT202201185181", DateOnly.FromDateTime(DateTime.UtcNow), "PLANN", "Medical", 100, ClaimStatus.Pending, null));
            claims.Add(new ClaimDto(1, 3, 4, 5, "LT202201185121", DateOnly.FromDateTime(DateTime.UtcNow), "PLANN", "Mental", 75, ClaimStatus.Approved, null));
            claims.Add(new ClaimDto(1, 3, 4, 5, "LT202201185141", DateOnly.FromDateTime(DateTime.UtcNow), "PLANN", "Dental", 50, ClaimStatus.Denied, null));

            var viewModel = new ClaimsSearchViewModel
            {
                Claims = claims,
                EmployerId = employerId,
                EmployerClaimFilling = true,
                Page = pageNumber ?? 1,
                TotalCount = 6,
                TotalPages = 2,
            };

            return PartialView("_Claims", viewModel);
        }
    }
}
