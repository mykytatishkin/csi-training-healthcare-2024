using CSI.IBTA.AuthService.DTOs.Errors;
using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.AuthService.DTOs;
using CSI.IBTA.Shared;

namespace CSI.IBTA.AuthService.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<LoginResponse>> Login(LoginRequest request)
        {
            var result = await _unitOfWork.Accounts.Find(a => a.Username == request.Username);

            if (!result.Any())
            {
                return new GenericResponse<LoginResponse>(true, Errors.InvalidCredentials, null);
            }

            var account = result.Single();

            bool isPasswordCorrect = _passwordHasher.Verify(request.Password, account.Password);

            if (isPasswordCorrect == false)
            {
                return new GenericResponse<LoginResponse>(true, Errors.InvalidCredentials, null);
            }

            var token = _jwtTokenGenerator.GenerateToken(account.Id, account.Role.ToString());

            return new GenericResponse<LoginResponse>(false, null, new LoginResponse(token));
        }
    }
}
