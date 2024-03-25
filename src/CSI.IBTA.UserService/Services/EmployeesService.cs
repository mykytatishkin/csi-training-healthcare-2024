using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.DTOs;
using AutoMapper;

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
    }
}
