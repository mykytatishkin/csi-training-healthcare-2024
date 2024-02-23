using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployersService
    {
        public Task<GenericHttpResponse<EmployerDto[]>> GetAll();

        public Task<GenericHttpResponse<EmployerDto>> GetEmployer(int employerId);

        public Task<GenericHttpResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto);

        public Task<GenericHttpResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto);

        public Task<GenericHttpResponse<bool>> DeleteEmployer(int employerId);

        public Task<GenericHttpResponse<IEnumerable<UserDto>>> GetEmployerUsers(int employerId);

        public Task<GenericHttpResponse<SettingsDto[]>> GetAllEmployerSettings(int employerId);

        public Task<GenericHttpResponse<bool?>> GetEmployerSettingValue(int employerId, string condition);

        public Task<GenericHttpResponse<SettingsDto[]>> UpdateEmployerSettings(int employerId, SettingsDto[] SettingsDtos);
    }
}