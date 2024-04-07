using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.UserService.Interfaces
{
    public interface IDecodingService
    {
        GenericResponse<EmployerEmployeeDto> GetDecodedEmployerEmployee(byte[] encryptedData);
    }
}
