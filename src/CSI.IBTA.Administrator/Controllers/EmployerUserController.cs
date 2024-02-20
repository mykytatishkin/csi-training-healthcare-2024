using AutoMapper;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Administrator.Controllers
{
    [Route("EmployerUsers")]
    public class EmployerUserController : Controller
    {
        private readonly IEmployerUserClient _employerUserClient;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public EmployerUserController(
            IEmployerUserClient employerUserClient,
            IJwtTokenService jwtTokenService,
            IMapper mapper)
        {
            _employerUserClient = employerUserClient;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("Admin")]
        public IActionResult Index(int employerId)
        {
            Console.WriteLine("Idzas: " + employerId);
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(int employerId, CreateEmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            Console.WriteLine("Anotha idz: " + employerId);
            Console.WriteLine("dgjdgd: " + model.Username);
            //var command = _mapper.Map<CreateEmployerUserCommand>(ModelState);

            var command = new CreateUserDto(
                model.Username,
                model.Password,
                model.Firstname,
                model.Lastname,
                Role.EmployerAdmin,
                employerId,
                "", "", "", "", "", "");

            var response = await _employerUserClient.CreateEmployerUser(command, token);

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Title);
            }

            return RedirectToAction("Index", new { employerId });
        }
    }
}
