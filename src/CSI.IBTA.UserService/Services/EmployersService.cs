using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSI.IBTA.UserService.Services
{
    internal class EmployersService : IEmployersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly List<(string key, bool value)> _defaultSettings;
        private readonly ILogger<EmployersService> _logger;

        public EmployersService(ILogger<EmployersService> logger, IConfiguration configuration, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _logger = logger;
            var defaultSettings = configuration.GetSection("DefaultEmployerSettings").GetChildren();
            if (defaultSettings == null)
                _logger.LogError("Default employer settings is missing in appsettings.json");

            _defaultSettings = new List<(string key, bool value)>();
            foreach (var setting in defaultSettings!)
            {
                if (!bool.TryParse(setting.Value, out bool val))
                {
                    _logger.LogError($"Failed to parse value for {setting.Key} setting");
                    throw new InvalidOperationException("Failed to parse employer settings");
                }
                _defaultSettings.Add((setting.Key, val));
            }
        }

        public async Task<GenericHttpResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto)
        {
            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new GenericHttpResponse<EmployerDto>(true, new HttpError("There is an employer with the same code and name combination", HttpStatusCode.BadRequest), null);
            }

            var e = new Employer()
            {
                Name = dto.Name,
                Code = dto.Code,
                Email = dto.Email,
                Phone = dto.Phone,
                State = dto.State,
                Street = dto.Street,
                City = dto.City,
                Zip = dto.ZipCode,
                Settings = _defaultSettings.Select(x =>
                    new Settings()
                    {
                        Condition = x.key,
                        State = x.value
                    }).ToList()
            };

            if (dto.LogoFile != null)
            {
                var res = _fileService.EncryptImage(dto.LogoFile);
                if (res.encryptedFile == null) return new GenericHttpResponse<EmployerDto>(true, new HttpError("Logo file is in incorrect format", HttpStatusCode.BadRequest), null);

                e.Logo = res.encryptedFile;
            }

            var success = await _unitOfWork.Employers.Add(e);
            if (!success)
                return new GenericHttpResponse<EmployerDto>(true, new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<EmployerDto>(false, null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericHttpResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericHttpResponse<EmployerDto>(true, new HttpError("Employer not found", HttpStatusCode.NotFound), null);

            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new GenericHttpResponse<EmployerDto>(true, new HttpError("There is an employer with the same code and name combination", HttpStatusCode.BadRequest), null);
            }

            e.Name = dto.Name;
            e.Code = dto.Code;
            e.Email = dto.Email;
            e.Phone = dto.Phone;
            e.State = dto.State;
            e.Street = dto.Street;
            e.City = dto.City;
            e.Zip = dto.ZipCode;

            if (dto.NewLogoFile != null)
            {
                var res = _fileService.EncryptImage(dto.NewLogoFile);
                if (res.encryptedFile == null) return new GenericHttpResponse<EmployerDto>(true, new HttpError("Logo file is in incorrect format", HttpStatusCode.BadRequest), null);

                e.Logo = res.encryptedFile;
            }

            var success = _unitOfWork.Employers.Upsert(e);
            if (!success)
                return new GenericHttpResponse<EmployerDto>(true, new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<EmployerDto>(false, null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericHttpResponse<bool>> DeleteEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericHttpResponse<bool>(true, new HttpError("Emplyer not found", HttpStatusCode.NotFound), false);

            var success = await _unitOfWork.Employers.Delete(e.Id);

            if (!success)
                return new GenericHttpResponse<bool>(true, new HttpError("Server failed to delete employer", HttpStatusCode.InternalServerError), false);

            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<bool>(false, null, true);
        }

        public async Task<GenericHttpResponse<EmployerDto>> GetEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericHttpResponse<EmployerDto>(true, new HttpError("Emplyer not found", HttpStatusCode.NotFound), null);

            return new GenericHttpResponse<EmployerDto>(false, null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericHttpResponse<EmployerDto[]>> GetAll()
        {
            var res = await _unitOfWork.Employers.All();

            if (res == null) return new GenericHttpResponse<EmployerDto[]>(true, new HttpError("Server failed to fetch employers", HttpStatusCode.InternalServerError), null);

            return new GenericHttpResponse<EmployerDto[]>(false, null, res.Select(e => new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo)).ToArray());
        }

        public async Task<GenericHttpResponse<IEnumerable<UserDto>>> GetEmployerUsers(int employerId)
        {
            var response = await _unitOfWork.Users.Find(u => u.EmployerId == employerId);

            if (response == null)
            {
                var error = new HttpError("Server failed to fetch employer users", HttpStatusCode.InternalServerError);
                return new GenericHttpResponse<IEnumerable<UserDto>>(true, error, null);
            }

            var userDtos = new List<UserDto>();
            foreach (User user in response)
            {
                var userAccount = await _unitOfWork.Accounts.GetById(user.Id);

                if (userAccount == null)
                {
                    var error = new HttpError("Server failed to fetch employer user's account", HttpStatusCode.InternalServerError);
                    return new GenericHttpResponse<IEnumerable<UserDto>>(true, error, null);
                }

                var userEmail = await _unitOfWork.Emails.GetById(user.Id);

                if (userEmail == null)
                {
                    var error = new HttpError("Server failed to fetch employer user's email", HttpStatusCode.InternalServerError);
                    return new GenericHttpResponse<IEnumerable<UserDto>>(true, error, null);
                }

                var userDto = new UserDto(
                    user.Id,
                    userAccount.Role,
                    userAccount.Username,
                    user.Firstname,
                    user.Lastname,
                    user.AccountId,
                    user.EmployerId,
                    userEmail.EmailAddress);

                userDtos.Add(userDto);
            }

            return new GenericHttpResponse<IEnumerable<UserDto>>(false, null, userDtos);
        }
    }
}
