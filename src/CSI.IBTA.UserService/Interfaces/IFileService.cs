
namespace CSI.IBTA.UserService.Interfaces
{
    public interface IFileService
    {
        public (string? encryptedFile, string description) EncryptImage(IFormFile image);
    }
}
