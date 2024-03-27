using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface IEmployersClient
    {
        Task<GenericResponse<EmployerDto>> GetEmployerById(int id);
    }
}