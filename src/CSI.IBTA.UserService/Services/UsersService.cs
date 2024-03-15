using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.UserService.Interfaces;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CSI.IBTA.DB.Migrations.Migrations;

namespace CSI.IBTA.UserService.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersService(IUserUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .ToListAsync();

            var userDtos = users.Select(_mapper.Map<UserDto>);
            return new GenericResponse<IEnumerable<UserDto>>(null, userDtos);
        }

        public async Task<GenericResponse<UserDto>> GetUserByAccountId(int accountId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(a => a.Account.Id == accountId);

            if (user == null)
            {
                return new GenericResponse<UserDto>(new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(null, _mapper.Map<UserDto>(user));
        }

        public async Task<GenericResponse<UserDto>> GetUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                return new GenericResponse<UserDto>(new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(null, _mapper.Map<UserDto>(user));
        }

        public async Task<GenericResponse<IEnumerable<UserDto>>> GetUsers(List<int> userIds)
        {
            var users = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Include(u => u.Emails)
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            return new GenericResponse<IEnumerable<UserDto>>(null, users.Select(_mapper.Map<UserDto>));
        }

        public async Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Username == createUserDto.UserName);

            if (existingAccount.Any())
            {
                var error = new HttpError("User with this username already exists", HttpStatusCode.Conflict);
                return new GenericResponse<NewUserDto>(error, null);
            }

            var conflictingEmail = await _unitOfWork.Users
                .Include(u => u.Account)
                .Where(u => u.Emails.First().EmailAddress == createUserDto.EmailAddress)
                .ToListAsync();

            if (conflictingEmail.Count != 0)
            {
                var error = new HttpError("User with this email already exists", HttpStatusCode.Conflict);
                return new GenericResponse<NewUserDto>(error, null);
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
            return new GenericResponse<NewUserDto>(null, _mapper.Map<NewUserDto>(newUser));
        }

        public async Task<GenericResponse<UpdatedUserDto>> PutUser(int userId, PutUserDto putUserDto)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Addresses)
                .Include(u => u.Emails)
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                var error = new HttpError("User was not found", HttpStatusCode.NotFound);
                return new GenericResponse<UpdatedUserDto>(error, null);
            }

            var conflictingUsername = await _unitOfWork.Accounts
                .Find(a => a.Username == putUserDto.UserName && a.Id != user.AccountId);

            if (conflictingUsername.Any())
            {
                var error = new HttpError("User with this username already exists", HttpStatusCode.Conflict);
                return new GenericResponse<UpdatedUserDto>(error, null);
            }

            var conflictingEmail = await _unitOfWork.Users
                .Include(u => u.Account)
                .Where(u => u.Emails.First().EmailAddress == putUserDto.EmailAddress && u.Account.Id != user.AccountId)
                .ToListAsync();
            
            if (conflictingEmail.Count != 0)
            {
                var error = new HttpError("User with this email already exists", HttpStatusCode.Conflict);
                return new GenericResponse<UpdatedUserDto>(error, null);
            }

            user.Account.Username = putUserDto.UserName;
            user.Account.Password = PasswordHasher.Hash(putUserDto.Password);
            user.Firstname = putUserDto.FirstName;
            user.Lastname = putUserDto.LastName;
            user.Addresses[0].State = putUserDto.AddressState;
            user.Addresses[0].Street = putUserDto.AddressStreet;
            user.Addresses[0].City = putUserDto.AddressCity;
            user.Addresses[0].Zip = putUserDto.AddressZip;
            user.Emails[0].EmailAddress = putUserDto.EmailAddress;
            user.Phones[0].PhoneNumber = putUserDto.PhoneNumber;

            await _unitOfWork.CompleteAsync();

            return new GenericResponse<UpdatedUserDto>(
                null,
                new UpdatedUserDto(
                    user.Id,
                    user.Account.Username,
                    user.Account.Password,
                    user.Firstname,
                    user.Lastname,
                    user.Account.Id,
                    user.Account.Role,
                    user.Phones[0].PhoneNumber,
                    user.Emails[0].EmailAddress,
                    user.Addresses[0].State,
                    user.Addresses[0].Street,
                    user.Addresses[0].City,
                    user.Addresses[0].Zip
                )
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
