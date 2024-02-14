using CSI.IBTA.Shared.DTOs;
using System.Security.Claims;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployersService
    {
        public Task<GenericResponse<EmployerDto>> GetEmployer(int employerId);
        public Task<GenericResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto);
        public Task<GenericResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto);
        public Task<GenericResponse<bool>> DeleteEmployer(int employerId);
    }
}
