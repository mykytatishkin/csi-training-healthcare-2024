using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Shared.DataStructures;
using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.Shared.DTOs;
using System.Net;
using System.Linq;

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
            string? currentNumberFilter,
            string? currentEmployerFilter,
            int? pageNumber,
            int? pageSize)
        {
            if (numberFilter != null || employerFilter != null)
            {
                pageNumber = 1;
            }

            numberFilter = numberFilter ?? currentNumberFilter;
            employerFilter = employerFilter ?? currentEmployerFilter;
            ViewData["CurrentNumberFilter"] = numberFilter;
            ViewData["CurrentEmployerFilter"] = employerFilter;

            var res = await _claimsClient.GetClaims();
            if (res.Result != null)
            {
                var claims = res.Result;

                if (!string.IsNullOrEmpty(numberFilter))
                {
                    claims = claims.Where(s => s.ClaimNumber.Contains(numberFilter));
                }
                if (!string.IsNullOrEmpty(employerFilter))
                {
                    claims = claims.Where(s => s.EmployerId.ToString().Equals(employerFilter));
                }

                ViewData["Page"] = "Home";
                var claimList = claims ?? new List<ClaimDto>().AsQueryable();
                var paginatedList = new PaginatedList<ClaimDto>(claimList, pageNumber ?? 1, pageSize ?? 8);
                return PartialView("_Claims", paginatedList);
            }

            if (res.Error.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Auth");
            }

            return PartialView("_Claims");
        }
    }
}