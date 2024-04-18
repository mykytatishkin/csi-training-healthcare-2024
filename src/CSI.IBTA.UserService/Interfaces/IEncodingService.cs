using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IEncodingService
    {
        Task<GenericResponse<byte[]>> GetEncodedEmployerEmployee(int employerId, int employeeId);
        Task<GenericResponse<byte[]>> GetEncodedEmployerEmployeeSettings(int employerId, int employeeId);
        GenericResponse<byte[]> Encode(object o);
    }
}
