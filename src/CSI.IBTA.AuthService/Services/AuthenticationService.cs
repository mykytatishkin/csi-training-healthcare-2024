﻿using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.Shared.DTOs.Login;
using CSI.IBTA.Shared.DTOs.Errors;

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
            var result = await _unitOfWork.Accounts.Find(a => a.Username == request.Username);

            if (!result.Any())
            {
                return new GenericResponse<LoginResponse>(HttpErrors.InvalidCredentials, null);
            }

            var account = result.Single();

            bool isPasswordCorrect = PasswordHasher.Verify(request.Password, account.Password);

            if (isPasswordCorrect == false)
            {
                return new GenericResponse<LoginResponse>(HttpErrors.InvalidCredentials, null);
            }

            var token = _jwtTokenGenerator.GenerateToken(account.Id, account.Role.ToString());

            return new GenericResponse<LoginResponse>(null, new LoginResponse(token));
        }
    }
}
