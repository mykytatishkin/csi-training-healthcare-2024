using CSI.IBTA.UserService.Interfaces;

namespace CSI.IBTA.UserService.Services
{
    internal class FileService : IFileService
    {
        private readonly string[] _allowedImageFormats;
        private readonly ILogger<FileService> _logger;
        public FileService(IConfiguration configuration, ILogger<FileService> logger)
        {
            _logger = logger;
            var allowedImageFormats = configuration.GetSection("AllowedImageFormats").Get<string[]>();
            if (allowedImageFormats == null || allowedImageFormats.Length == 0)
            {
                _logger.LogError("AllowedImageFormats is missing in appsettings.json");
                throw new InvalidOperationException("AllowedImageFormats is missing in appsettings.json");
            }
            _allowedImageFormats = allowedImageFormats;
        }
        public (string? encryptedFile, string description) EncryptImage(IFormFile image)
        {
            string ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            var correctFormat = _allowedImageFormats.Contains(ext);

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
