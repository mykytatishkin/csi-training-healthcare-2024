using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.UserService.Types;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployersService
    {
        public Task<ServiceResponse<EmployerDto?>> GetEmployerProfile(int employerId);
        public Task<ServiceResponse<EmployerDto?>> CreateEmployer(CreateEmployerDto dto);
        public Task<ServiceResponse<EmployerDto?>> UpdateEmployer(int employerId, UpdateEmployerDto dto);
        public Task<ServiceResponse<bool>> DeleteEmployer(int employerId);
    }
}
