using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.Shared.DTOs.Login;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.AuthService.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserUnitOfWork _unitOfWork;

        public AuthenticationService(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserUnitOfWork unitOfWork)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<LoginResponse>> Login(LoginRequest request)
        {
            var account = await _unitOfWork.Accounts.GetSet()
                .FirstOrDefaultAsync(a => a.Username == request.Username);

            if (account == null) 
                return new GenericResponse<LoginResponse>(HttpErrors.InvalidCredentials, null);

            bool isPasswordCorrect = PasswordHasher.Verify(request.Password, account.Password);

            if (!isPasswordCorrect) 
                return new GenericResponse<LoginResponse>(HttpErrors.InvalidCredentials, null);

            (int? employerId, int? userId) = (null, null);
            if (account.Role != Role.Administrator)
            {
                var user = await _unitOfWork.Users.GetSet().FirstOrDefaultAsync(x => x.AccountId == account.Id);
                if (user == null) return new GenericResponse<LoginResponse>(HttpErrors.ResourceNotFound, null);

                if (account.Role == Role.EmployerAdmin) employerId = user.EmployerId;
                else if (account.Role == Role.Employee) userId = user.Id;
            }

            var token = _jwtTokenGenerator.GenerateToken(account.Id, employerId, userId, account.Role.ToString());

            return new GenericResponse<LoginResponse>(null, new LoginResponse(token));
        }
    }
}
