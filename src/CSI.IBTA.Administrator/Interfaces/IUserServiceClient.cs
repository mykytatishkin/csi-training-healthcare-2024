using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.Shared.Types;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserServiceClient
    {
        Task<IQueryable<Employer>?> GetEmployers();
        Task<TaskResult<EmployerDto?>> CreateEmployer(CreateEmployerDto dto);
        Task<TaskResult<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId);
        Task<GenericInternalResponse<EmployerDto>> GetEmployerById(int id);
    }
}
