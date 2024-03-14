using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.DataStructures;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;
using System.Net;
using System.Linq;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Administrator.Constants;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly IClaimsClient _claimsClient;

        public HomeController(IUserServiceClient userServiceClient, IClaimsClient claimsClient)
        {
            _userServiceClient = userServiceClient;
            _claimsClient = claimsClient;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("Employers")]
        public async Task<IActionResult> GetEmployers(
            string? nameFilter,
            string? codeFilter,
            string? currentNameFilter,
            string? currentCodeFilter,
            int? pageNumber,
            int? pageSize)
        {
            if (nameFilter != null || codeFilter != null)
            {
                pageNumber = 1;
            }

            nameFilter = nameFilter ?? currentNameFilter;
            codeFilter = codeFilter ?? currentCodeFilter;
            ViewData["CurrentNameFilter"] = nameFilter;
            ViewData["CurrentCodeFilter"] = codeFilter;

            var res = await _userServiceClient.GetEmployers();
            if (res.Result != null)
            {
                var employers = res.Result;

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    employers = employers.Where(s => s.Name.Contains(nameFilter));
                }
                if (!string.IsNullOrEmpty(codeFilter))
                {
                    employers = employers.Where(s => s.Code.Equals(codeFilter));
                }

                ViewData["Page"] = "Home";
                var employerList = employers ?? new List<EmployerDto>().AsQueryable();
                var paginatedList = new PaginatedList<EmployerDto>(employerList, pageNumber ?? 1, pageSize ?? 8);
                return PartialView("_Employer", paginatedList);
            }

            if (res.Error.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Auth");
            }

            return PartialView("_Employer");
        }

        [HttpGet("Claims")]
        public async Task<IActionResult> GetClaims(
            string? numberFilter,
            string? employerFilter,
            string? claimStatusFilter,
            string? currentNumberFilter,
            string? currentEmployerFilter,
            string? currentClaimStatusFilter,
            int? pageNumber)
        {
            if (numberFilter != null || employerFilter != null)
            {
                pageNumber = 1;
            }

            numberFilter = numberFilter ?? currentNumberFilter;
            employerFilter = employerFilter ?? currentEmployerFilter;
            claimStatusFilter = claimStatusFilter ?? currentClaimStatusFilter;
            ViewData["CurrentNumberFilter"] = numberFilter;
            ViewData["CurrentEmployerFilter"] = employerFilter;
            ViewData["CurrentClaimStatusFilter"] = claimStatusFilter;

            var claimsResponse = await _claimsClient.GetClaims(
                pageNumber ?? 1, 
                PaginationConstants.ClaimsPerPage, 
                numberFilter ?? "", 
                employerFilter ?? "", 
                claimStatusFilter ?? "");

            if (claimsResponse.Error != null || claimsResponse.Result == null)
            {
                return PartialView("_Claims");
            }

            var claims = claimsResponse.Result.Claims;

            var userIds = claims.Select(c => c.EmployeeId).ToList();
            var usersResponse = await _userServiceClient.GetUsers(userIds);

            if (usersResponse.Error != null || usersResponse.Result == null)
            {
                return PartialView("_Claims");
            }

            var users = usersResponse.Result;

            var employersResponse = await _userServiceClient.GetEmployers();

            if (employersResponse.Error != null || employersResponse.Result == null)
            {
                return PartialView("_Claims");
            }

            var employers = employersResponse.Result;

            var combinedClaims = claims.Select(c => new ViewClaimDto(
                c.Id,
                c.EmployeeId,
                users.Where(u => u.Id == c.EmployeeId)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .First(),
                c.EmployerId,
                employers.Where(e => e.Id == c.EmployerId)
                    .Select(e => e.Name)
                    .First(),
                c.ClaimNumber,
                c.DateOfService,
                c.PlanTypeName,
                c.Amount,
                c.Status));

            ViewData["Page"] = "Home";

            var viewModel = new ClaimsSearchViewModel
            {
                Claims = combinedClaims,
                Employers = employers,
                Page = pageNumber ?? 1,
                TotalCount = claimsResponse.Result.TotalCount,
                TotalPages = claimsResponse.Result.TotalPages,
            };

            return PartialView("_Claims", viewModel);
        }
    }
}