using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IDecodingService
    {
        GenericResponse<EmployerEmployeeDto> GetDecodedEmployerEmployee(byte[] encryptedData);
    }
}
