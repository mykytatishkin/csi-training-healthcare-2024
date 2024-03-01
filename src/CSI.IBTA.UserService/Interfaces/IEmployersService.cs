using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployersService
    {
        public Task<GenericResponse<EmployerDto[]>> GetAll();
        public Task<GenericResponse<EmployerDto>> GetEmployer(int employerId);
        public Task<GenericResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto);
        public Task<GenericResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto);
        public Task<GenericResponse<bool>> DeleteEmployer(int employerId);
        public Task<GenericResponse<SettingsDto[]>> GetAllEmployerSettings(int employerId);
        public Task<GenericResponse<bool?>> GetEmployerSettingValue(int employerId, string condition);
        public Task<GenericResponse<SettingsDto[]>> UpdateEmployerSettings(int employerId, SettingsDto[] SettingsDtos);
    }
}
