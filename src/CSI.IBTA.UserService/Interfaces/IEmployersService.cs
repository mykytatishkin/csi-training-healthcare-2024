using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEmployersService
    {
        public Task<GenericResponse<IEnumerable<EmployerDto>>> GetAll();
        public Task<GenericResponse<PagedEmployersResponse>> GetEmployersFiltered(int page, int pageSize, string nameFilter = "", string codeFilter = "");
        public Task<GenericResponse<EmployerDto>> GetEmployer(int employerId);
        public Task<GenericResponse<EmployerDto>> GetEmployerByAccountId(int accountId);
        public Task<GenericResponse<IEnumerable<EmployerDto>>> GetEmployers(List<int> employerIds);
        public Task<GenericResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto);
        public Task<GenericResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto);
        public Task<GenericResponse<bool>> DeleteEmployer(int employerId);
        public Task<GenericResponse<IEnumerable<UserDto>>> GetEmployerUsers(int employerId);
        public Task<GenericResponse<SettingsDto[]>> GetAllEmployerSettings(int employerId);
        public Task<GenericResponse<SettingsWithEmployerStateDto?>> GetEmployerSetting(int employerId, string condition);
        public Task<GenericResponse<SettingsDto[]>> UpdateEmployerSettings(int employerId, SettingsDto[] SettingsDtos);
        public Task<GenericResponse<SettingsWithEmployerStateDto>> UpdateEmployerClaimSetting(int employerId, UpdateClaimSettingDto employerSettingState);
    }
}