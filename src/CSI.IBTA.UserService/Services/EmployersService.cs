using AutoMapper;
using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.Constants;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CSI.IBTA.UserService.Services
{
    internal class EmployersService : IEmployersService
    {
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly List<(string key, bool value)> _defaultSettings;
        private readonly ILogger<EmployersService> _logger;
        private readonly IMapper _mapper;

        public EmployersService(ILogger<EmployersService> logger, IConfiguration configuration, IUserUnitOfWork unitOfWork, IFileService fileService, IMapper mapper)
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
            _mapper = mapper;
        }

        public async Task<GenericResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto)
        {
            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new GenericResponse<EmployerDto>(new HttpError("There is an employer with the same code and name combination", HttpStatusCode.BadRequest), null);
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
                if (res.encryptedFile == null) return new GenericResponse<EmployerDto>(new HttpError("Logo file is in incorrect format", HttpStatusCode.BadRequest), null);

                e.Logo = res.encryptedFile;
            }

            var success = await _unitOfWork.Employers.Add(e);
            if(!success)
                return new GenericResponse<EmployerDto>(new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _unitOfWork.CompleteAsync();
            return new GenericResponse<EmployerDto>(null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if(e == null) return new GenericResponse<EmployerDto>(new HttpError("Employer not found", HttpStatusCode.NotFound), null);

            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code && employerId != x.Id);

            if (hasSameCombination.Any())
            {
                return new GenericResponse<EmployerDto>(new HttpError("There is an employer with the same code and name combination", HttpStatusCode.BadRequest), null);
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
                if (res.encryptedFile == null) return new GenericResponse<EmployerDto>(new HttpError("Logo file is in incorrect format", HttpStatusCode.BadRequest), null);

                e.Logo = res.encryptedFile;
            }

            var success = _unitOfWork.Employers.Upsert(e);
            if (!success)
                return new GenericResponse<EmployerDto>(new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _unitOfWork.CompleteAsync();
            return new GenericResponse<EmployerDto>(null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericResponse<bool>> DeleteEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericResponse<bool>(new HttpError("Emplyer not found", HttpStatusCode.NotFound), false);

            var success = await _unitOfWork.Employers.Delete(e.Id);

            if (!success)
                return new GenericResponse<bool>(new HttpError("Server failed to delete employer", HttpStatusCode.InternalServerError), false);
            
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<bool>(null, true);
        }

        public async Task<GenericResponse<EmployerDto>> GetEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericResponse<EmployerDto>(new HttpError("Employer not found", HttpStatusCode.NotFound), null);

            return new GenericResponse<EmployerDto>(null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericResponse<IEnumerable<EmployerDto>>> GetEmployers(List<int> employerIds)
        {
            var employers = await _unitOfWork.Employers.Find(e => employerIds.Contains(e.Id));

            return new(null, employers.Select(e => new EmployerDto(
                e.Id,
                e.Name,
                e.Code,
                e.Email,
                e.Street,
                e.City,
                e.State,
                e.Zip,
                e.Phone,
                e.Logo)));
        }

        public async Task<GenericResponse<IEnumerable<EmployerDto>>> GetAll()
        {
            var res = await _unitOfWork.Employers.All();

            if (res == null) return new GenericResponse<IEnumerable<EmployerDto>>(new HttpError("Server failed to fetch employers", HttpStatusCode.InternalServerError), null);

            return new GenericResponse<IEnumerable<EmployerDto>>(null, res.Select(_mapper.Map<EmployerDto>));
        }

        public async Task<GenericResponse<PagedEmployersResponse>> GetEmployersFiltered(int page = 1, int pageSize = 8, string nameFilter = "", string codeFilter = "")
        {
            var filteredEmployers = _unitOfWork.Employers.GetSet()
            .Where(e => (nameFilter == "" || e.Name.Contains(nameFilter))
                && (codeFilter == "" || e.Code.Equals(codeFilter)));

            var totalCount = filteredEmployers.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var employers = await filteredEmployers
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var responseList = new PagedEmployersResponse(employers.Select(_mapper.Map<EmployerDto>).ToList(), page, pageSize, totalPages, totalCount);

            return new GenericResponse<PagedEmployersResponse>(null, responseList);
        }

        public async Task<GenericResponse<IEnumerable<UserDto>>> GetEmployerUsers(int employerId)
        {
            var response = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Emails)
                .Include(u => u.Phones)
                .Where(u => u.EmployerId == employerId)
                .ToListAsync();

            var userDtos = response.Select(_mapper.Map<UserDto>);

            return new GenericResponse<IEnumerable<UserDto>>(null, userDtos);
        }

        public async Task<GenericResponse<SettingsDto[]>> GetAllEmployerSettings(int employerId)
        {
            var e = await _unitOfWork.Settings.Find(s => s.EmployerId == employerId);

            if (e == null) return new GenericResponse<SettingsDto[]>(new HttpError("Settings not found", HttpStatusCode.NotFound), null);

            return new GenericResponse<SettingsDto[]>(null, e.Select(s => new SettingsDto(s.Condition, s.State)).ToArray());
        }

        public async Task<GenericResponse<SettingsWithEmployerStateDto?>> GetEmployerSetting(int employerId, string condition)
        {
            bool wrongCondition = true;
            foreach (var s in _defaultSettings)
                if (s.key.Equals(condition))
                {
                    wrongCondition = false;
                    break;
                }
            if (wrongCondition)
                return new GenericResponse<SettingsWithEmployerStateDto?>(new HttpError("Wrong settings type", HttpStatusCode.NotFound), null);

            var employerSetting = (await _unitOfWork.Settings.Find(s => s.EmployerId == employerId && s.Condition.Equals(condition)))?.First();
            if (employerSetting == null) return new GenericResponse<SettingsWithEmployerStateDto?>(new HttpError("Settings not found", HttpStatusCode.NotFound), null);
            var settingDto = new SettingsWithEmployerStateDto(employerSetting.Condition, employerSetting.State, employerSetting.EmployerState);

            return new GenericResponse<SettingsWithEmployerStateDto?>(null, settingDto);
        }

        public async Task<GenericResponse<SettingsDto[]>> UpdateEmployerSettings(int employerId, SettingsDto[] SettingsDtos)
        {
            var e = await _unitOfWork.Employers.Include(e => e.Settings)
                .SingleOrDefaultAsync(s => s.Id == employerId);
            if (e == null) return new GenericResponse<SettingsDto[]>(new HttpError("Employer not found", HttpStatusCode.NotFound), null);

            foreach (var newSetting in e.Settings)
            {
                foreach (var formSetting in SettingsDtos)
                {
                    if (newSetting.Condition.Equals(formSetting.Condition))
                    {
                        newSetting.State = formSetting.State;
                    }
                }
            }

            var success = _unitOfWork.Employers.Upsert(e);
            if (!success)
                return new GenericResponse<SettingsDto[]>(new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);
            await _unitOfWork.CompleteAsync();

            return new GenericResponse<SettingsDto[]>(null, e.Settings.Select(s => new SettingsDto(s.Condition, s.State)).ToArray());
        }

        public async Task<GenericResponse<SettingsWithEmployerStateDto>> UpdateEmployerClaimSetting(int employerId, UpdateClaimSettingDto updateClaimSettingDto)
        {
            var employer = await _unitOfWork.Employers.Include(e => e.Settings)
                .SingleOrDefaultAsync(s => s.Id == employerId);
            if (employer == null) return new GenericResponse<SettingsWithEmployerStateDto>(new HttpError("Employer not found", HttpStatusCode.NotFound), null);


            var setting = employer.Settings.Where(s => s.Condition.Equals(EmployerConstants.ClaimFilling)).FirstOrDefault();
            if (setting == null)
            {
                return new GenericResponse<SettingsWithEmployerStateDto>(new HttpError("Setting not found", HttpStatusCode.NotFound), null);
            }

            setting.EmployerState = updateClaimSettingDto.EmployerState;

            var success = _unitOfWork.Employers.Upsert(employer);
            if (!success)
                return new GenericResponse<SettingsWithEmployerStateDto>(new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);
            await _unitOfWork.CompleteAsync();

            return new GenericResponse<SettingsWithEmployerStateDto>(null, new SettingsWithEmployerStateDto(setting.Condition, setting.State, setting.EmployerState));
        }
    }
}