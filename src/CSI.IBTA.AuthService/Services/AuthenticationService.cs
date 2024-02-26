using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.AuthService.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericHttpResponse<LoginResponse>> Login(LoginRequest request)
        {
            var result = await _unitOfWork.Accounts.Find(a => a.Username == request.Username);

            if (!result.Any())
            {
                return new GenericHttpResponse<LoginResponse>(true, HttpErrors.InvalidCredentials, null);
            }

            var account = result.Single();

            bool isPasswordCorrect = PasswordHasher.Verify(request.Password, account.Password);

            if (isPasswordCorrect == false)
            {
                return new GenericHttpResponse<LoginResponse>(true, HttpErrors.InvalidCredentials, null);
            }

            var token = _jwtTokenGenerator.GenerateToken(account.Id, account.Role.ToString());

            return new GenericHttpResponse<LoginResponse>(false, null, new LoginResponse(token));
        }
    }
}
