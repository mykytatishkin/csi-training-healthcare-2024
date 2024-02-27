using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.UserService.Interfaces;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.UserService.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService( 
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<UserDto[]>> GetAllUsers()
        {
            var userList = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Select(user => new UserDto(user.Id, user.Account.Role, user.Account.Username, 
                user.Firstname, user.Lastname, user.Account.Id,
                user.Employer == null ? null : user.Employer.Id))
                .ToArrayAsync();
            
            return new GenericResponse<UserDto[]>(null, userList);
        }

        public async Task<GenericResponse<UserDto>> GetUserByAccountId(int accountId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .FirstOrDefaultAsync(a => a.Account.Id == accountId);
            
            if (user == null)
            {
                return new GenericResponse<UserDto>(new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(null,
                new UserDto(user.Id, user.Account.Role, user.Account.Username, user.Firstname, user.Lastname, 
                user.Account.Id, user.Employer == null ? -1 : user.Employer.Id)
                );
        }

        public async Task<GenericResponse<UserDto>> GetUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                return new GenericResponse<UserDto>(new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(null,
                new UserDto(user.Id, user.Account.Role, user.Account.Username, user.Firstname, user.Lastname,
                user.Account.Id, user.Employer == null ? -1 : user.Employer.Id)
                );
        }

        public async Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Username == createUserDto.UserName);
            
            if (existingAccount.Any())
            {
                return new GenericResponse<NewUserDto>(new HttpError("User already exists", HttpStatusCode.UnprocessableEntity), null);
            }

            User newUser = new User()
            {
                Firstname = createUserDto.FirstName,
                Lastname = createUserDto.LastName,
                Account = new Account()
                {
                    Username = createUserDto.UserName,
                    Password = PasswordHasher.Hash(createUserDto.Password),
                    Role = createUserDto.Role
                },
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        State = createUserDto.AddressState,
                        Street = createUserDto.AddressStreet,
                        City = createUserDto.AddressCity,
                        Zip = createUserDto.AddressZip,
                    }
                },
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        EmailAddress = createUserDto.EmailAddress,
                    }
                },
                Phones = new List<Phone>()
                {
                    new Phone()
                    {
                        PhoneNumber = createUserDto.PhoneNumber,
                    }
                }
            };

            if (createUserDto.EmployerId != null)
            {
                Employer? employer = await _unitOfWork.Employers.GetById((int)createUserDto.EmployerId);
                if (employer == null)
                {
                    return new GenericResponse<NewUserDto>(new HttpError("Employer not found", HttpStatusCode.NotFound), null);
                } else
                {
                    newUser.Employer = employer;
                }
            }

            await _unitOfWork.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<NewUserDto>(null,
                new NewUserDto(newUser.Id, newUser.Account.Username, newUser.Account.Password
                , newUser.Firstname, newUser.Lastname, newUser.Account.Id, newUser.Employer == null ? null : newUser.Employer.Id, newUser.Account.Role
                , newUser.Phones[0].PhoneNumber, newUser.Emails[0].EmailAddress
                , newUser.Addresses[0].State, newUser.Addresses[0].Street, newUser.Addresses[0].City, newUser.Addresses[0].Zip)
            );
        }

        public async Task<GenericResponse<UpdatedUserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Addresses)
                .Include(u => u.Emails)
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                return new GenericResponse<UpdatedUserDto>(new HttpError("User not found", HttpStatusCode.NotFound), null);
            }


            if (updateUserDto.UserName != null)
            {
                var sameUsernameAccount = await _unitOfWork.Accounts.Find(a => a.Username == updateUserDto.UserName);
                if (sameUsernameAccount.Any())
                {
                    return new GenericResponse<UpdatedUserDto>(new HttpError("Account with new username already exists. Remove username from request paramaters if changing username is not intended.", HttpStatusCode.UnprocessableEntity), null);
                }
                user.Account.Username = updateUserDto.UserName;
            }

            user.Account.Password = PasswordHasher.Hash(updateUserDto.Password);
            user.Firstname = updateUserDto.FirstName;
            user.Lastname = updateUserDto.LastName;
            user.Addresses[0].State = updateUserDto.AddressState;
            user.Addresses[0].Street = updateUserDto.AddressStreet;
            user.Addresses[0].City = updateUserDto.AddressCity;
            user.Addresses[0].Zip = updateUserDto.AddressZip;
            user.Emails[0].EmailAddress = updateUserDto.EmailAddress;
            user.Phones[0].PhoneNumber = updateUserDto.PhoneNumber;

            _unitOfWork.Users.Upsert(user);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<UpdatedUserDto>(null,
                new UpdatedUserDto(user.Id, user.Account.Username, user.Account.Password
                , user.Firstname, user.Lastname, user.Account.Id, user.Account.Role
                , user.Phones[0].PhoneNumber, user.Emails[0].EmailAddress
                , user.Addresses[0].State, user.Addresses[0].Street, user.Addresses[0].City, user.Addresses[0].Zip)
                );
        }

        public async Task<GenericResponse<bool>> DeleteUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new GenericResponse<bool>(new HttpError("User not found", HttpStatusCode.NotFound), false);
            }

            await _unitOfWork.Accounts.Delete(user.Account.Id);
            await _unitOfWork.Users.Delete(user.Id);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<bool>(null, true);
        }
    }
}
