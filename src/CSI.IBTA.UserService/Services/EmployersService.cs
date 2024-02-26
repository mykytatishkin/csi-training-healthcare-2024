﻿using AutoMapper;
using CSI.IBTA.DataLayer.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly List<(string key, bool value)> _defaultSettings;
        private readonly ILogger<EmployersService> _logger;
        private readonly IMapper _mapper;

        public EmployersService(ILogger<EmployersService> logger, IConfiguration configuration, IUnitOfWork unitOfWork, IFileService fileService, IMapper mapper)
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
            return new GenericHttpResponse<EmployerDto>(false, null, _mapper.Map<EmployerDto>(e));
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
            return new GenericHttpResponse<EmployerDto>(false, null, _mapper.Map<EmployerDto>(e));
        }

        public async Task<GenericHttpResponse<bool>> DeleteEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericHttpResponse<bool>(true, new HttpError("Employer not found", HttpStatusCode.NotFound), false);

            var success = await _unitOfWork.Employers.Delete(e.Id);

            if (!success)
                return new GenericHttpResponse<bool>(true, new HttpError("Server failed to delete employer", HttpStatusCode.InternalServerError), false);

            await _unitOfWork.CompleteAsync();
            return new GenericHttpResponse<bool>(false, null, true);
        }

        public async Task<GenericHttpResponse<EmployerDto>> GetEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericHttpResponse<EmployerDto>(true, new HttpError("Employer not found", HttpStatusCode.NotFound), null);

            return new GenericHttpResponse<EmployerDto>(false, null, _mapper.Map<EmployerDto>(e));
        }

        public async Task<GenericHttpResponse<IEnumerable<EmployerDto>>> GetAll()
        {
            var res = await _unitOfWork.Employers.All();

            if (res == null) return new GenericHttpResponse<IEnumerable<EmployerDto>>(true, new HttpError("Server failed to fetch employers", HttpStatusCode.InternalServerError), null);

            return new GenericHttpResponse<IEnumerable<EmployerDto>>(false, null, res.Select(_mapper.Map<EmployerDto>));
        }

        public async Task<GenericHttpResponse<IEnumerable<UserDto>>> GetEmployerUsers(int employerId)
        {
            var response = await _unitOfWork.Users
                .Include(u => u.Account)
                .Include(u => u.Emails)
                .Where(u => u.EmployerId == employerId)
                .ToListAsync();

            var userDtos = response.Select(_mapper.Map<UserDto>);

            return new GenericHttpResponse<IEnumerable<UserDto>>(false, null, userDtos);
        }

        public async Task<GenericHttpResponse<SettingsDto[]>> GetAllEmployerSettings(int employerId)
        {
            var e = await _unitOfWork.Settings.Find(s => s.EmployerId == employerId);

            if (e == null) return new GenericHttpResponse<SettingsDto[]>(true, new HttpError("Settings not found", HttpStatusCode.NotFound), null);

            return new GenericHttpResponse<SettingsDto[]>(false, null, e.Select(s => new SettingsDto(s.Condition, s.State)).ToArray());
        }

        public async Task<GenericHttpResponse<bool?>> GetEmployerSettingValue(int employerId, string condition)
        {
            bool wrongCondition = true;
            foreach (var s in _defaultSettings)
                if (s.key.Equals(condition))
                {
                    wrongCondition = false;
                    break;
                }
            if (wrongCondition)
                return new GenericHttpResponse<bool?>(true, new HttpError("Wrong settings type", HttpStatusCode.NotFound), null);

            var e = await _unitOfWork.Settings.Find(s => s.EmployerId == employerId && s.Condition.Equals(condition));
            if (e == null) return new GenericHttpResponse<bool?>(true, new HttpError("Settings not found", HttpStatusCode.NotFound), null);

            return new GenericHttpResponse<bool?>(false, null, e.First().State);
        }

        public async Task<GenericHttpResponse<SettingsDto[]>> UpdateEmployerSettings(int employerId, SettingsDto[] SettingsDtos)
        {
            var e = await _unitOfWork.Employers.Include(e => e.Settings)
                .FirstOrDefaultAsync(s => s.Id == employerId);
            if (e == null) return new GenericHttpResponse<SettingsDto[]>(true, new HttpError("Settings not found", HttpStatusCode.NotFound), null);

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
                return new GenericHttpResponse<SettingsDto[]>(true, new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);
            await _unitOfWork.CompleteAsync();

            return new GenericHttpResponse<SettingsDto[]>(false, null, e.Settings.Select(s => new SettingsDto(s.Condition, s.State)).ToArray());
        }
    }
}