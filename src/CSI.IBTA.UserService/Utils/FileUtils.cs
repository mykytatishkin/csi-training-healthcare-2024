using CSI.IBTA.Shared.Settings;

namespace CSI.IBTA.UserService.Utils
{
    internal static class FileUtils
    {
        public static (string? encryptedFile, string description) EncryptImage(IFormFile image)
        {
            var allowedFormats = ImagesSettings.GetAllowedFormats();
            string ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            var correctFormat = allowedFormats.Contains(ext);

            if (!correctFormat) return (null, "File is in incorrect format");


            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();
                var converted = Convert.ToBase64String(bytes);
                return (converted, "File has been converted successfully");
            }
        }

    }
}
