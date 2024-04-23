using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net;
using CSI.IBTA.Shared.Utils;
using CSI.IBTA.Shared.Constants;

namespace CSI.IBTA.UserService.Services
{
    internal class EmployeesService : IEmployeesService
    {
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IMapper _mapper;

        public EmployeesService(IUserUnitOfWork userUnitOfWork, IMapper mapper)
        {
            _userUnitOfWork = userUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<PagedEmployeesResponse>> GetEmployees(int page, int pageSize, int employerId, string firstname = "", string lastname = "", string ssn = "")
        {
            var filteredEmployees = _userUnitOfWork.Users.GetSet()
                .Include(c => c.Account)
                .Where(c => c.EmployerId == employerId)
                .Where(c => c.Account.Role == Role.Employee)
                .Where(c => firstname == "" || c.Firstname.ToLower().Contains(firstname.ToLower()))
                .Where(c => lastname == "" || c.Lastname.ToLower().Contains(lastname.ToLower()))
                .Where(c => ssn == "" || c.SSN != null && c.SSN.ToLower().Contains(ssn.ToLower()));

            var totalCount = filteredEmployees.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var employees = await filteredEmployees
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var employeeDtos = employees.Select(_mapper.Map<EmployeeDto>).ToList();

            var response = new PagedEmployeesResponse(employeeDtos, page, pageSize, totalPages, totalCount);
            return new GenericResponse<PagedEmployeesResponse>(null, response);
        }

        public async Task<GenericResponse<FullEmployeeDto>> GetEmployee(int employeeId)
        {
            var employee = await _userUnitOfWork.Users.GetSet()
                .Include(u => u.Addresses)
                .Include(u => u.Account)
                .Include(u => u.Phones)
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(u => u.Id == employeeId);

            if (employee == null)
            {
                return new GenericResponse<FullEmployeeDto>(new HttpError("Employee not found", HttpStatusCode.NotFound), null);
            }

            var address = employee.Addresses is { Count: > 0 } ? employee.Addresses[0] : null;

            var employeeDto = new FullEmployeeDto(
                employee.Id,
                employee.Account.Username,
                employee.Account.Password,
                employee.Firstname,
                employee.Lastname,
                employee.SSN!,
                employee.Phones is { Count: > 0 } ? employee.Phones[0].PhoneNumber : "",
                DateOnly.FromDateTime(employee.DateOfBirth.GetValueOrDefault()),
                employee.Emails is { Count: > 0 } ? employee.Emails[0].EmailAddress : "",
                address?.State ?? "",
                address?.Street ?? "",
                address?.City ?? "",
                address?.Zip ?? "",
                employee.EmployerId.GetValueOrDefault());

            return new GenericResponse<FullEmployeeDto>(null, employeeDto);
        }

        public async Task<GenericResponse<FullEmployeeDto>> CreateEmployee(CreateEmployeeDto dto)
        {
            var addConsumerSetting = (await _userUnitOfWork.Settings.Find(s => s.EmployerId == dto.EmployerId
                && s.Condition.Equals(EmployerConstants.AddConsumers))).SingleOrDefault();

            if (addConsumerSetting == null || !addConsumerSetting.IsAllowed)
            {
                return new GenericResponse<FullEmployeeDto>(new HttpError("Administrator has forbidden adding consumers. Try again later.", HttpStatusCode.BadRequest), null);
            }

            bool hasSameSSN = await _userUnitOfWork.Users.GetSet().AnyAsync(x => x.SSN == dto.SSN);
            string invalidValues = "";
            if (hasSameSSN)
            {
                invalidValues += "SSN";
            }

            bool hasSameUsername = await _userUnitOfWork.Users.GetSet().AnyAsync(x => x.Account.Username == dto.UserName);
            if (hasSameUsername)
            {
                invalidValues += invalidValues == "" ? "" : ", ";
                invalidValues += "username";
            }

            if (invalidValues != "")
            {
                return new GenericResponse<FullEmployeeDto>(new HttpError($"An employee already exists with the same: {invalidValues}.", HttpStatusCode.BadRequest), null);
            }

            var user = new User()
            {
                Firstname = dto.FirstName,
                Lastname = dto.LastName,
                DateOfBirth = dto.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                Account = new Account
                {
                    Username = dto.UserName,
                    Password = PasswordHasher.Hash(dto.Password),
                    Role = Role.Employee,
                },
                Addresses = new List<Address>
                {
                    new Address()
                    {
                        State = dto.AddressState,
                        Street = dto.AddressStreet,
                        City = dto.AddressCity,
                        Zip = dto.AddressZip,
                    }
                },
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        EmailAddress = dto.Email
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone()
                    {
                        PhoneNumber = dto.PhoneNumber
                    }
                },
                SSN = dto.SSN,
                Employer = await _userUnitOfWork.Employers.GetById(dto.EmployerId),
                EmployerId = dto.EmployerId
            };

            var success = await _userUnitOfWork.Users.Add(user);
            if (!success)
                return new GenericResponse<FullEmployeeDto>(new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _userUnitOfWork.CompleteAsync();
            return new GenericResponse<FullEmployeeDto>(null, new FullEmployeeDto(user.Id, user.Account.Username, user.Account.Password, user.Firstname, user.Lastname, user.SSN, user.Phones.FirstOrDefault()!.PhoneNumber, DateOnly.FromDateTime(user.DateOfBirth.GetValueOrDefault()), user.Emails.FirstOrDefault()!.EmailAddress, user.Addresses.FirstOrDefault()!.State, user.Addresses.FirstOrDefault()!.Street, user.Addresses.FirstOrDefault()!.City, user.Addresses.FirstOrDefault()!.Zip, (int)user.EmployerId));
        }

        public async Task<GenericResponse<FullEmployeeDto>> UpdateEmployee(UpdateEmployeeDto dto)
        {
            var user = await _userUnitOfWork.Users.GetSet()
                .Include(u => u.Addresses)
                .Include(u => u.Account)
                .Include(u => u.Phones)
                .Include(u => u.Emails)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (user == null)
            {
                return new GenericResponse<FullEmployeeDto>(new HttpError("Employee not found", HttpStatusCode.NotFound), null);
            }

            bool hasSameSSN = await _userUnitOfWork.Users.GetSet().AnyAsync(x => x.SSN == dto.SSN && x.Id != dto.Id);
            if (hasSameSSN)
            {
                return new GenericResponse<FullEmployeeDto>(new HttpError("An employee already exists with the same SSN.", HttpStatusCode.BadRequest), null);
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Account.Password = PasswordHasher.Hash(dto.Password);
            }
            user.Firstname = dto.FirstName;
            user.Lastname = dto.LastName;
            user.DateOfBirth = dto.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            user.SSN = dto.SSN;
            user.Addresses[0].State = dto.AddressState;
            user.Addresses[0].Street = dto.AddressStreet;
            user.Addresses[0].City = dto.AddressCity;
            user.Addresses[0].Zip = dto.AddressZip;
            user.Phones[0].PhoneNumber = dto.PhoneNumber;
            user.Emails[0].EmailAddress = dto.Email;

            await _userUnitOfWork.CompleteAsync();

            return new GenericResponse<FullEmployeeDto>(null, new FullEmployeeDto(user.Id, user.Account.Username, user.Account.Password, user.Firstname, user.Lastname, user.SSN, user.Phones.FirstOrDefault()!.PhoneNumber, DateOnly.FromDateTime(user.DateOfBirth.GetValueOrDefault()), user.Emails.FirstOrDefault()!.EmailAddress, user.Addresses.FirstOrDefault()!.State, user.Addresses.FirstOrDefault()!.Street, user.Addresses.FirstOrDefault()!.City, user.Addresses.FirstOrDefault()!.Zip, (int)user.EmployerId!));
        }

        public async Task<GenericResponse<IEnumerable<UserDto>>> GetEmployeesByUsernames(List<string> usernames, int employerId)
        {
            var users = await _userUnitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Employer)
                .Include(u => u.Emails)
                .Include(u => u.Phones)
                .Where(u => u.EmployerId == employerId)
                .Where(u => usernames.Contains(u.Account.Username))
                .ToListAsync();

            return new GenericResponse<IEnumerable<UserDto>>(null, users.Select(_mapper.Map<UserDto>));
        }

        public async Task<GenericResponse<bool>> GetAllowClaimFilling(int employeeId)
        {
            var employee = await _userUnitOfWork.Users
                .Include(x => x.Employer!)
                .Include(x => x.Employer!.Settings)
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            if(employee == null) return new GenericResponse<bool>(HttpErrors.ResourceNotFound, false);

            var setting = employee.Employer?.Settings.FirstOrDefault(x => x.Condition == EmployerConstants.ClaimFilling);
            return new GenericResponse<bool>(null, setting?.IsAllowed ?? false);
        }
    }
}