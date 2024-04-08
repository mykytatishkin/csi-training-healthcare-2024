using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Consumer.Interfaces
{
    public interface IEmployersClient
    {
        Task<GenericResponse<EmployerDto?>> UpdateEmployer(UpdateEmployerDto dto);
        Task<GenericResponse<EmployerDto>> GetEmployerById(int id);
        Task<GenericResponse<EmployerDto>> GetEmployerByAccountId(int id);
    }
}