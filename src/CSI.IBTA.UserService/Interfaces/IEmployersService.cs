using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployersService
    {
        public Task<GenericHttpResponse<IEnumerable<EmployerDto>>> GetAll();
        public Task<GenericHttpResponse<EmployerDto>> GetEmployer(int employerId);
        public Task<GenericHttpResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto);
        public Task<GenericHttpResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto);
        public Task<GenericHttpResponse<bool>> DeleteEmployer(int employerId);
        public Task<GenericHttpResponse<IEnumerable<UserDto>>> GetEmployerUsers(int employerId);
    }
}