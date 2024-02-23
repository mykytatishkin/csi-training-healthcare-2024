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

        public async Task<GenericResponse<UserDto>> GetUserByAccountId(int accountId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .FirstOrDefaultAsync(a => a.Account.Id == accountId);

            if (user == null)
            {
                return new GenericResponse<UserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(false, null, _mapper.Map<UserDto>(user));
        }

        public async Task<GenericResponse<UserDto>> GetUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                return new GenericResponse<UserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            return new GenericResponse<UserDto>(false, null, _mapper.Map<UserDto>(user));
        }

        public async Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Username == createUserDto.UserName);

            if (existingAccount.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User already exists", HttpStatusCode.UnprocessableEntity), null);
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
                    return new GenericResponse<NewUserDto>(true, new HttpError("Employer not found", HttpStatusCode.NotFound), null);
                }
                else
                {
                    newUser.Employer = employer;
                }
            }

            await _unitOfWork.Users.Add(newUser);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<NewUserDto>(false, null, _mapper.Map<NewUserDto>(newUser));
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
                return new GenericResponse<UpdatedUserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }

            if (updateUserDto.UserName != null)
            {
                var sameUsernameAccount = await _unitOfWork.Accounts.Find(a => a.Username == updateUserDto.UserName);
                if (sameUsernameAccount.Any())
                {
                    return new GenericResponse<UpdatedUserDto>(true, new HttpError("Account with new username already exists. Remove username from request paramaters if changing username is not intended.", HttpStatusCode.UnprocessableEntity), null);
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
            return new GenericResponse<UpdatedUserDto>(false, null, _mapper.Map<UpdatedUserDto>(user));
        }

        public async Task<GenericResponse<bool>> DeleteUser(int userId)
        {
            var user = await _unitOfWork.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new GenericResponse<bool>(true, new HttpError("User not found", HttpStatusCode.NotFound), false);
            }

            await _unitOfWork.Accounts.Delete(user.Account.Id);
            await _unitOfWork.Users.Delete(user.Id);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<bool>(false, null, true);
        }
    }
}
