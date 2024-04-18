using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.BenefitsService.Interfaces
{
    public interface IDecodingService
    {
        GenericResponse<T> GetDecodedData<T>(byte[] encryptedData) where T : class;
    }
}
