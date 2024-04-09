using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using CSI.IBTA.Shared.DTOs.Errors;

namespace CSI.IBTA.UserService.Services
{
    internal class EncodingService : IEncodingService
    {
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly ILogger<EncodingService> _logger;
        private readonly byte[] _aesKey;

        public EncodingService(IUserUnitOfWork userUnitOfWork, IConfiguration configuration, ILogger<EncodingService> logger)
        {
            _userUnitOfWork = userUnitOfWork;
            _logger = logger;
            var aesKey = configuration.GetSection("AesKey").Get<byte[]>();
            if (aesKey == null || aesKey.Length == 0)
            {
                _logger.LogError("AesKey is missing in appsettings.json");
                throw new InvalidOperationException("AesKey is missing in appsettings.json");
            }
            _aesKey = aesKey;
        }

        public async Task<GenericResponse<byte[]>> GetEncodedEmployerEmployee(int employerId, int employeeId)
        {
            var employee = (await _userUnitOfWork.Users.Find(x => x.Id == employeeId && x.EmployerId == employerId))
                .FirstOrDefault();

            if (employee == null) return new GenericResponse<byte[]>(HttpErrors.ResourceNotFound, null);

            var dto = new EmployerEmployeeDto(employerId, employeeId);

            using (MemoryStream memoryStream = new MemoryStream())
            using (Aes aes = Aes.Create())
            {
                aes.Key = _aesKey;
                aes.GenerateIV();

                var json = JsonSerializer.Serialize(dto);
                var jsonBytes = Encoding.UTF8.GetBytes(json);

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(jsonBytes, 0, jsonBytes.Length);
                    cryptoStream.FlushFinalBlock();
                }

                var ivPlusEncrypted = new byte[aes.IV.Length + memoryStream.ToArray().Length];
                Array.Copy(aes.IV, ivPlusEncrypted, aes.IV.Length);
                Array.Copy(memoryStream.ToArray(), 0, ivPlusEncrypted, aes.IV.Length, memoryStream.ToArray().Length);

                return new GenericResponse<byte[]>(null, ivPlusEncrypted);
            }
        }
    }
}
