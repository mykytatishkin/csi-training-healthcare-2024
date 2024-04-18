using CSI.IBTA.Shared.DTOs;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using CSI.IBTA.BenefitsService.Interfaces;

namespace CSI.IBTA.BenefitsService.Services
{
    internal class DecodingService : IDecodingService
    {
        private readonly ILogger<DecodingService> _logger;
        private readonly byte[] _aesKey;

        public DecodingService(IConfiguration configuration, ILogger<DecodingService> logger)
        {
            _logger = logger;
            var aesKey = configuration.GetSection("AesKey").Get<byte[]>();
            if (aesKey == null || aesKey.Length == 0)
            {
                _logger.LogError("AesKey is missing in appsettings.json");
                throw new InvalidOperationException("AesKey is missing in appsettings.json");
            }
            _aesKey = aesKey;
        }

        public GenericResponse<T> GetDecodedData<T>(byte[] encryptedData) where T : class 
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _aesKey;
                var iv = new byte[aes.IV.Length];
                Array.Copy(encryptedData, iv, aes.IV.Length);
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedData, aes.IV.Length, encryptedData.Length - aes.IV.Length);
                        cryptoStream.FlushFinalBlock();

                        memoryStream.Position = 0;
                        using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
                        {
                            string decryptedJson = reader.ReadToEnd();
                            var deserialized = JsonSerializer.Deserialize<T>(decryptedJson);
                            return new GenericResponse<T>(null, deserialized);
                        }
                    }
                }
            }
        }
    }
}
