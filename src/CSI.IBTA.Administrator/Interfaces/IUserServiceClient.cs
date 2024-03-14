using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserServiceClient
    {
        Task<GenericResponse<PagedEmployersResponse>> GetEmployers(int page, int pageSize, string nameFilter = "", string codeFilter = "");
        Task<GenericResponse<UserDto>> GetUser(int userId);
        Task<GenericResponse<IEnumerable<UserDto>>> GetUsers(List<int> userIds);
        Task<GenericResponse<IQueryable<SettingsDto>?>> GetEmployerSettings(int employerId);
        Task<GenericResponse<IQueryable<SettingsDto>?>> UpdateEmployerSettings(int employerId, List<SettingsDto>? model);
        Task<GenericResponse<EmployerDto?>> CreateEmployer(CreateEmployerDto dto);
        Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId);
        Task<GenericResponse<EmployerDto>> GetEmployerById(int id);
        Task<GenericResponse<IEnumerable<EmployerDto>>> GetEmployersByIds(List<int> employerIds);
        Task<GenericResponse<List<UserDto>>> GetEmployerUsers(int employerId);
        Task<GenericResponse<bool?>> CreateEmployerUser(CreateUserDto command);
        Task<GenericResponse<bool?>> UpdateEmployerUser(PutUserDto command, int accountId);
    }
}