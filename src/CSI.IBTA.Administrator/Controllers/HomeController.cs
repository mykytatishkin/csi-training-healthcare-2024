using CSI.IBTA.Administrator.Filters;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Shared.DataStructures;
using CSI.IBTA.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CSI.IBTA.Administrator.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class HomeController : Controller
    {
        private readonly IUserServiceClient _userServiceClient;

        public HomeController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public PartialViewResult GetPartial(string partialName)
        {
            return PartialView($"_{partialName}");
        }

        // Replace your existing Index action method with the provided code
        public async Task<IActionResult> Index(
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
                return View(new PaginatedList<EmployerDto>(employers ?? new List<EmployerDto>().AsQueryable(), pageNumber ?? 1, pageSize ?? 8));
            }
            if (res.Error != null && res.Error.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Index", "Auth");

            return View("Index");
        }
    }
}
