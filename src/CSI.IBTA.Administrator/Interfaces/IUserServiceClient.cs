using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserServiceClient
    {
        Task<List<Employer>?> GetEmployers(string token);
        Task<GenericInternalResponse<UserDto>> GetUser(int userId);
        Task<IQueryable<SettingsDto>?> GetEmployerSettings(int employerId);
        Task<IQueryable<SettingsDto>?> UpdateEmployerSettings(int employerId, List<SettingsDto>? model);
        Task<TaskResult<EmployerDto?>> CreateEmployer(CreateEmployerDto dto);
        Task<TaskResult<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId);
        Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id);
    }
}