using Azure.Core;
using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.UserService.Services
{
    public class UserServiceS : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserServiceS( 
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto?> GetUser(int accountId)
        {
            await _unitOfWork.Accounts.All();
            var result = await _unitOfWork.Users.Find(a => a.Account.Id == accountId);
            
            if (!result.Any())
            {
                return null;
            }

            var user = result.First();
            return new UserDto(user.Id, user.Account.Username, user.Firstname, user.Lastname, user.Account.Id);
        }

        public async Task<NewUserDto?> CreateUser(CreateUserDto createUserDto)
        {
            var existingUser = await _unitOfWork.Users.Find(a => a.Account.Username == createUserDto.UserName);
            
            if (existingUser.Any())
            {
                return null;
            }

            Account newAccount = new Account()
            {
                Username = createUserDto.UserName,
                Password = _passwordHasher.Hash(createUserDto.Password),
                RoleId = createUserDto.RoleId
            };

            User newUser = new User()
            {
                Firstname = createUserDto.FirstName,
                Lastname = createUserDto.LastName,
                Account = newAccount,
            };

            await _unitOfWork.Accounts.Add(newAccount);
            await _unitOfWork.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();
            return new NewUserDto(newUser.Id, newUser.Account.Username, newUser.Account.Password, newUser.Firstname, newUser.Lastname, newUser.Account.Id, newUser.Account.RoleId);
        }

        public async Task<NewUserDto?> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            await _unitOfWork.Accounts.All();
            var existingUser = await _unitOfWork.Users.Find(a => a.Id == userId);

            if (!existingUser.Any())
            {
                return null;
            }

            var user = existingUser.First();

            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Id == user.Account.Id);

            if (!existingAccount.Any())
            {
                return null;
            }

            var account = existingAccount.First();
            account.Username = updateUserDto.UserName;
            account.Password = _passwordHasher.Hash(updateUserDto.Password);


            user.Firstname = updateUserDto.FirstName;
            user.Lastname = updateUserDto.LastName;
            user.Account = account;

            _unitOfWork.Accounts.Upsert(account);
            _unitOfWork.Users.Upsert(user);
            await _unitOfWork.CompleteAsync();
            return new NewUserDto(user.Id, user.Account.Username, user.Account.Password, user.Firstname, user.Lastname, user.Account.Id, user.Account.RoleId);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            await _unitOfWork.Accounts.All();
            var existingUser = await _unitOfWork.Users.Find(a => a.Id == userId);

            if (!existingUser.Any())
            {
                return false;
            }

            var user = existingUser.First();

            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Id == user.Account.Id);

            if (existingAccount.Any())
            {
                await _unitOfWork.Accounts.Delete(existingAccount.First().Id);
            }
            await _unitOfWork.Users.Delete(user.Id);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
