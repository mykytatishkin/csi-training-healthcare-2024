using CSI.IBTA.AuthService.Exceptions;
using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared;

namespace CSI.IBTA.AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
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

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = await _unitOfWork.Accounts.Find(a => a.Username == request.Username);

            if (!result.Any())
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            var account = result.Single();

            bool isPasswordCorrect = _passwordHasher.Verify(request.Password, account.Password);

            if (isPasswordCorrect == false)
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            var token = _jwtTokenGenerator.GenerateToken(account.Id, account.Role.ToString());

            return new LoginResponse(token);
        }
    }
}
