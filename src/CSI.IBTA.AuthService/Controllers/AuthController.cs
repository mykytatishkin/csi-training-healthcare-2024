using CSI.IBTA.AuthService.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.IConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.AuthService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "Login")]
        public async Task<IActionResult> Login(LoginRequest query)
        {
            var account = await _unitOfWork.Accounts.GetById(2);
            var employer = await _unitOfWork.Employers.GetById(1);

            bool success = await _unitOfWork.Users.Upsert(new User
            {
                Id = 3,
                Firstname = "TestasUP",
                Lastname = "Testas2Up",
                Account = account,
                Employer = employer
            });

            await _unitOfWork.CompleteAsync();

            return success ? Ok("OK") : BadRequest("Failed");
        }
    }
}
