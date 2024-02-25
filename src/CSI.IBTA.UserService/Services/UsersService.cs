using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.UserService.Interfaces;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CSI.IBTA.UserService.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
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
            return new GenericResponse<IEnumerable<UserDto>>(false, null, userDtos);
        }

        public async Task<GenericHttpResponse<UserDto>> GetUserByAccountId(int accountId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(a => a.Account.Id == accountId);

            if (user == null)
            {
                return new GenericHttpResponse<UserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(false, null, _mapper.Map<UserDto>(user));
        }

        public async Task<GenericHttpResponse<UserDto>> GetUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                return new GenericHttpResponse<UserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(false, null, _mapper.Map<UserDto>(user));
        }

        public async Task<GenericHttpResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Username == createUserDto.UserName);

            if (existingAccount.Any())
            {
                return new GenericHttpResponse<NewUserDto>(true, new HttpError("User already exists", HttpStatusCode.Conflict), null);
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
                    return new GenericHttpResponse<NewUserDto>(true, new HttpError("Employer not found", HttpStatusCode.NotFound), null);
                }
                else
                {
                    newUser.Employer = employer;
                }
            }

            await _unitOfWork.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<NewUserDto>(false, null, _mapper.Map<NewUserDto>(newUser));
        }

        public async Task<GenericHttpResponse<UpdatedUserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Addresses)
                .Include(u => u.Emails)
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                return new GenericHttpResponse<UpdatedUserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            if (updateUserDto.UserName != null)
            {
                var sameUsernameAccount = await _unitOfWork.Accounts.Find(a => a.Username == updateUserDto.UserName);
                if (sameUsernameAccount.Any())
                {
                    return new GenericHttpResponse<UpdatedUserDto>(true, new HttpError("Account with new username already exists. Remove username from request paramaters if changing username is not intended.", HttpStatusCode.UnprocessableEntity), null);
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
            return new GenericHttpResponse<UpdatedUserDto>(false, null, _mapper.Map<UpdatedUserDto>(user));
        }

        public async Task<GenericHttpResponse<UpdatedUserDto>> PutUser(int userId, PutUserDto putUserDto)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Addresses)
                .Include(u => u.Emails)
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new GenericHttpResponse<UpdatedUserDto>(
                    true, HttpErrors.ResourceNotFound, null
                );
            }

            var conflictingUser = await _unitOfWork.Accounts
                .Find(a => a.Username == putUserDto.UserName && a.Id != user.AccountId);

            if (conflictingUser.Any())
            {
                return new GenericHttpResponse<UpdatedUserDto>(
                    true, HttpErrors.Conflict, null
                );
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

            return new GenericHttpResponse<UpdatedUserDto>(
                false,
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


        public async Task<GenericHttpResponse<bool>> DeleteUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new GenericHttpResponse<bool>(true, new HttpError("User not found", HttpStatusCode.NotFound), false);
            }

            await _unitOfWork.Accounts.Delete(user.Account.Id);
            await _unitOfWork.Users.Delete(user.Id);
            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<bool>(false, null, true);
        }
    }
}
