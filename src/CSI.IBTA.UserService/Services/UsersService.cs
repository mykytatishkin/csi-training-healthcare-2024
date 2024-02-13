using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.UserService.Interfaces;
using System.Net;
using System.Security.Claims;

namespace CSI.IBTA.UserService.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService( 
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<UserDto>> GetUser(int accountId, HttpContext httpContext)
        {
            await _unitOfWork.Accounts.All();
            var result = await _unitOfWork.Users.Find(a => a.Account.Id == accountId);
            
            if (!result.Any())
            {
                return new GenericResponse<UserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }
            var user = result.First();
            if (!IsSuperiorRole(httpContext.User, user.Account.Role))
            {
                return new GenericResponse<UserDto>(true, new HttpError("Invalid User Role", HttpStatusCode.Unauthorized), null);
            }
            return new GenericResponse<UserDto>(false, null,
                new UserDto(user.Id, user.Account.Username, user.Firstname, user.Lastname, user.Account.Id)
                );
        }

        public async Task<GenericResponse<NewUserDto>> CreateUser(CreateUserDto createUserDto, HttpContext httpContext)
        {
            if (!IsNextSuperiorRole(httpContext.User, createUserDto.Role))
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("Invalid User Role", HttpStatusCode.UnprocessableEntity), null);
            }

            var existingUser = await _unitOfWork.Users.Find(a => a.Account.Username == createUserDto.UserName);
            
            if (existingUser.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User already exists", HttpStatusCode.UnprocessableEntity), null);
            }

            Account newAccount = new Account()
            {
                Username = createUserDto.UserName,
                Password = PasswordHasher.Hash(createUserDto.Password),
                Role = createUserDto.Role
            };

            User newUser = new User()
            {
                Firstname = createUserDto.FirstName,
                Lastname = createUserDto.LastName,
                Account = newAccount,
            };

            Address newUserAddress = new Address()
            {
                State = createUserDto.AddressState,
                Street = createUserDto.AddressStreet,
                City = createUserDto.AddressCity,
                Zip = createUserDto.AddressZip,
                Account = newUser
            };

            Email newUserEmail = new Email()
            {
                EmailAddress = createUserDto.EmailAddress,
                Account = newUser,
            };

            Phone newUserPhone = new Phone()
            {
                PhoneNumber = createUserDto.PhoneNumber,
                Account = newUser,
            };

            await _unitOfWork.Accounts.Add(newAccount);
            await _unitOfWork.Users.Add(newUser);
            await _unitOfWork.Addresses.Add(newUserAddress);
            await _unitOfWork.Emails.Add(newUserEmail);
            await _unitOfWork.Phones.Add(newUserPhone);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<NewUserDto>(false, null,
                new NewUserDto(newUser.Id, newUser.Account.Username, newUser.Account.Password
                , newUser.Firstname, newUser.Lastname, newUser.Account.Id, newUser.Account.Role
                , newUserPhone.PhoneNumber, newUserEmail.EmailAddress
                , newUserAddress.State, newUserAddress.Street, newUserAddress.City, newUserAddress.Zip)
                );                
        }

        public async Task<GenericResponse<NewUserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto, HttpContext httpContext)
        {
            await _unitOfWork.Accounts.All();
            var existingUser = await _unitOfWork.Users.Find(a => a.Id == userId);

            if (!existingUser.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User not found", HttpStatusCode.NotFound), null);
            }
            var user = existingUser.First();
            var s = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.Parse(s) != user.Account.Id && !IsNextSuperiorRole(httpContext.User, user.Account.Role))
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User is unauthorized", HttpStatusCode.Unauthorized), null);
            }

            var sameUsernameAccount = await _unitOfWork.Users.Find(a => a.Account.Username == updateUserDto.UserName && a.Id != userId);

            if (sameUsernameAccount.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("Account with new username already exists", HttpStatusCode.UnprocessableEntity), null);
            }

            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Id == user.Account.Id);

            if (!existingAccount.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("Account not found", HttpStatusCode.NotFound), null);
            }

            var userAddress = await _unitOfWork.Addresses.Find(a => a.Account.Id == user.Id);
            if (!userAddress.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User address not found", HttpStatusCode.NotFound), null);
            }
            var address = userAddress.First();

            var userEmail = await _unitOfWork.Emails.Find(a => a.Account.Id == user.Id);
            if (!userEmail.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User email not found", HttpStatusCode.NotFound), null);
            }
            var email = userEmail.First();

            var userPhone= await _unitOfWork.Phones.Find(a => a.Account.Id == user.Id);
            if (!userPhone.Any())
            {
                return new GenericResponse<NewUserDto>(true, new HttpError("User phone not found", HttpStatusCode.NotFound), null);
            }
            var phone = userPhone.First();

            var account = existingAccount.First();
            account.Username = updateUserDto.UserName;
            account.Password = PasswordHasher.Hash(updateUserDto.Password);


            user.Firstname = updateUserDto.FirstName;
            user.Lastname = updateUserDto.LastName;
            user.Account = account;

            address.State = updateUserDto.AddressState;
            address.Street = updateUserDto.AddressStreet;
            address.City = updateUserDto.AddressCity;
            address.Zip = updateUserDto.AddressZip;
            email.EmailAddress = updateUserDto.EmailAddress;
            phone.PhoneNumber = updateUserDto.PhoneNumber;

            _unitOfWork.Accounts.Upsert(account);
            _unitOfWork.Users.Upsert(user);
            _unitOfWork.Addresses.Upsert(address);
            _unitOfWork.Emails.Upsert(email);
            _unitOfWork.Phones.Upsert(phone);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<NewUserDto>(false, null,
                new NewUserDto(user.Id, user.Account.Username, user.Account.Password
                , user.Firstname, user.Lastname, user.Account.Id, user.Account.Role
                , phone.PhoneNumber, email.EmailAddress
                , address.State, address.Street, address.City, address.Zip)
                );
        }

        public async Task<GenericResponse<bool>> DeleteUser(int userId, HttpContext httpContext)
        {
            await _unitOfWork.Accounts.All();
            var existingUser = await _unitOfWork.Users.Find(a => a.Id == userId);

            if (!existingUser.Any())
            {
                return new GenericResponse<bool>(true, new HttpError("User not found", HttpStatusCode.NotFound), false);
            }
            var user = existingUser.First();

            if (!IsNextSuperiorRole(httpContext.User, user.Account.Role))
            {
                return new GenericResponse<bool>(true, new HttpError("User role invalid", HttpStatusCode.Unauthorized), false);
            }

            var userAddress = await _unitOfWork.Addresses.Find(a => a.Account.Id == user.Id);
            if (userAddress.Any())
            {
                await _unitOfWork.Addresses.Delete(userAddress.First().Id);
            }

            var userEmail = await _unitOfWork.Emails.Find(a => a.Account.Id == user.Id);
            if (userEmail.Any())
            {
                await _unitOfWork.Emails.Delete(userEmail.First().Id);
            }

            var userPhone = await _unitOfWork.Phones.Find(a => a.Account.Id == user.Id);
            if (!userPhone.Any())
            {
                await _unitOfWork.Phones.Delete(userPhone.First().Id);
            }

            var existingAccount = await _unitOfWork.Accounts.Find(a => a.Id == user.Account.Id);

            if (existingAccount.Any())
            {
                await _unitOfWork.Accounts.Delete(existingAccount.First().Id);
            }

            await _unitOfWork.Users.Delete(user.Id);
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<bool>(false, null, true);
        }

        private bool IsNextSuperiorRole(Role authUserRole, Role managedUserRole)
        {
            return (authUserRole == Role.Administrator && managedUserRole == Role.EmployerAdmin)
                || (authUserRole == Role.EmployerAdmin && managedUserRole == Role.Employee);
        }

        private bool IsNextSuperiorRole(ClaimsPrincipal authenticatedUser, Role managedUserRole)
        {
            Enum.TryParse(authenticatedUser.FindFirstValue(ClaimTypes.Role), out Role authUserRole);
            return IsNextSuperiorRole(authUserRole, managedUserRole);
        }

        private bool IsSuperiorRole(ClaimsPrincipal authenticatedUser, Role managedUserRole)
        {
            Enum.TryParse(authenticatedUser.FindFirstValue(ClaimTypes.Role), out Role authUserRole);
            return managedUserRole != Role.Administrator && 
                (IsNextSuperiorRole(authUserRole, managedUserRole) 
                || authUserRole == Role.Administrator);
        }
    }
}
