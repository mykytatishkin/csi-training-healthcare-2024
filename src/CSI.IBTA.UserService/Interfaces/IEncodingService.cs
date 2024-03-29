using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEncodingService
    {
        Task<GenericResponse<byte[]>> GetEncodedEmployerEmployee(int employerId, int employeeId);
    }
}
