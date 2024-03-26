using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;
using CSI.IBTA.Shared.DTOs.Errors;
using System.Net;
using CSI.IBTA.Shared.Utils;

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

        public async Task<GenericResponse<CreateEmployeeDto>> CreateEmployee(CreateEmployeeDto dto)
        {
            bool hasSameSSN = await _userUnitOfWork.Users.GetSet().AnyAsync(x => x.SSN == dto.SSN);
            if (hasSameSSN)
            {
                return new GenericResponse<CreateEmployeeDto>(new HttpError("An employee already exists with the same SSN.", HttpStatusCode.BadRequest), null);
            }

            bool hasSameName = await _userUnitOfWork.Users.GetSet().AnyAsync(x => x.Firstname == dto.FirstName && x.Lastname == dto.LastName);
            if (hasSameName)
            {
                return new GenericResponse<CreateEmployeeDto>(new HttpError("An employee already exists with the same name.", HttpStatusCode.BadRequest), null);
            }

            bool hasSameUsername = await _userUnitOfWork.Users.GetSet().AnyAsync(x => x.Account.Username == dto.UserName);
            if (hasSameUsername)
            {
                return new GenericResponse<CreateEmployeeDto>(new HttpError("An employee already exists with the same username.", HttpStatusCode.BadRequest), null);
            }

            var e = new User()
            {
                Firstname = dto.FirstName,
                Lastname = dto.LastName,
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
                Emails = new List<Email>(),
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

            var success = await _userUnitOfWork.Users.Add(e);
            if (!success)
                return new GenericResponse<CreateEmployeeDto>(new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _userUnitOfWork.CompleteAsync();
            return new GenericResponse<CreateEmployeeDto>(null, new CreateEmployeeDto(e.Account.Username, e.Account.Password, e.Firstname, e.Lastname, e.SSN, e.Phones.FirstOrDefault().PhoneNumber, e.Addresses.FirstOrDefault().State, e.Addresses.FirstOrDefault().Street, e.Addresses.FirstOrDefault().City, e.Addresses.FirstOrDefault().Zip, (int)e.EmployerId));
        }
    }
}