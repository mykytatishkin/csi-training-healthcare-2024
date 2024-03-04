using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IEmployerClient
    {
        Task<GenericResponse<EmployerDto>> GetEmployerById(int id);
    }
}
