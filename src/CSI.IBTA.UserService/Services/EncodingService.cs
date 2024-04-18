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
        private readonly IEmployersService _employerService;

        public EncodingService(IUserUnitOfWork userUnitOfWork, IConfiguration configuration, ILogger<EncodingService> logger, IEmployersService employerService)
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
            _employerService = employerService;
        }

        public GenericResponse<byte[]> Encode(object o)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (Aes aes = Aes.Create())
            {
                aes.Key = _aesKey;
                aes.GenerateIV();

                var json = JsonSerializer.Serialize(o);
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

        public async Task<GenericResponse<byte[]>> GetEncodedEmployerEmployee(int employerId, int employeeId)
        {
            var employee = (await _userUnitOfWork.Users.Find(x => x.Id == employeeId && x.EmployerId == employerId))
                .FirstOrDefault();

            if (employee == null) return new GenericResponse<byte[]>(HttpErrors.ResourceNotFound, null);

            var dto = new EmployerEmployeeDto(employerId, employeeId);

            return Encode(dto);
        }

        public async Task<GenericResponse<byte[]>> GetEncodedEmployerEmployeeSettings(int employerId, int employeeId)
        {
            var employee = (await _userUnitOfWork.Users.Find(x => x.Id == employeeId && x.EmployerId == employerId))
               .FirstOrDefault();

            if (employee == null) return new GenericResponse<byte[]>(HttpErrors.ResourceNotFound, null);

            var settingsResponse = await _employerService.GetAllEmployerSettings(employerId);
            if (settingsResponse.Result == null) new GenericResponse<byte[]>(settingsResponse.Error, null);

            var dto = new EmployerEmployeeSettingsDto(employerId, employeeId, settingsResponse.Result?.ToDictionary(x => x.Condition, x => x.State)!);

            return Encode(dto);
        }
    }
}
