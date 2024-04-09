using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IEmployersClient
    {
        Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto, int employerId);
        Task<GenericResponse<EmployerWithConsumerSettingDto>> GetEmployerById(int id);
    }
}