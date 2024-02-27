using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IUserServiceClient
    {
        Task<GenericResponse<IQueryable<EmployerDto>?>> GetEmployers();
        Task<GenericResponse<EmployerDto?>> CreateEmployer(CreateEmployerDto dto);
        Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId);
        Task<GenericResponse<EmployerDto>> GetEmployerById(int id);
    }
}
