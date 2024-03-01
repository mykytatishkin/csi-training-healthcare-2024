using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Types;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserServiceClient
    {
        Task<GenericHttpResponse<IQueryable<EmployerDto>?>> GetEmployers();
        Task<GenericInternalResponse<UserDto>> GetUser(int userId);
        Task<IQueryable<SettingsDto>?> GetEmployerSettings(int employerId);
        Task<IQueryable<SettingsDto>?> UpdateEmployerSettings(int employerId, List<SettingsDto>? model);
        Task<TaskResult<EmployerDto?>> CreateEmployer(CreateEmployerDto dto);
        Task<TaskResult<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId);
        Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id);
    }
}