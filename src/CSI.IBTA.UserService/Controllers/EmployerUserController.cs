using Microsoft.AspNetCore.Mvc;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.UserService.Interfaces;

namespace CSI.IBTA.UserService.Controllers
{
    [Route("api/v1/Employer/{employerId}/[controller]")]
    [ApiController]
    public class EmployerUserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _userService;
        private readonly IEmployerUsersService _employerUsersService;

        public EmployerUserController(IUsersService userService, IUnitOfWork unitOfWork, IEmployerUsersService employerUsersService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _employerUsersService = employerUsersService;
        }

        //[HttpPost]
        //[Authorize(Roles = $"{nameof(Role.Administrator)}")]
        //public async Task<IActionResult> PostEmployerUser(int employerId, CreateEmployerUserRequest request)
        //{
        //    //var createUserRequest = new CreateUserDto
        //    //{
        //    //    UserName = request.UserName,
        //    //    Password = request.Password,
        //    //    FirstName = request.FirstName,
        //    //    LastName = request.LastName
        //    //};

            

        //    //var response = await _userService.CreateUser(request.User, HttpContext);

        //    //if (response.Error != null)
        //    //{
        //    //    return Problem(
        //    //        title: response.Error.Title,
        //    //        statusCode: (int)response.Error.StatusCode
        //    //    );
        //    //}

        //    //if (response.Result == null)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    //var user = await _unitOfWork.Users.GetById(response.Result.Id);

        //    //if (user == null)
        //    //{
        //    //    return NotFound("User was not sent to database");
        //    //}

        //    //var employer = await _unitOfWork.Employers.GetById(employerId);

        //    //if (employer == null)
        //    //{
        //    //    return NotFound("Employer not found");
        //    //}

        //    //await _employerUsersService.EmployUser(user, employer);
        //    //return Ok();
        //}
    }
}
