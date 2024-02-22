﻿using CSI.IBTA.Administrator.Extensions;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DataStructures;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserServiceClient _userServiceClient;

        public HomeController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

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

            var employers = await _userServiceClient.GetEmployers();
            if (employers != null) 
            {
                if (!String.IsNullOrEmpty(nameFilter))
                {
                    employers = employers.Where(s => s.Name.Contains(nameFilter));
                }
                if (!String.IsNullOrEmpty(codeFilter))
                {
                    employers = employers.Where(s => s.Code.Equals(codeFilter));
                }
            }

            var paginatedEmployers = new PaginatedList<Employer>(employers ?? new List<Employer>().AsQueryable(), pageNumber ?? 1, pageSize ?? 8);

            return View(new HomeViewModel() { Employers = paginatedEmployers });
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployer(HomeViewModel model)
        {
            var res = await _userServiceClient.CreateEmployer(model.CreateEmployerViewModel.ToDto());
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Description);
                return View("Index", model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
