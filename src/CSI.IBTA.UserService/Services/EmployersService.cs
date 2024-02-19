using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Errors;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.UserService.Utils;
using System.Net;

namespace CSI.IBTA.UserService.Services
{
    internal class EmployersService : IEmployersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<EmployerDto>> CreateEmployer(CreateEmployerDto dto)
        {
            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new GenericResponse<EmployerDto>(true, new HttpError("There is an employer with the same code and name combination", HttpStatusCode.BadRequest), null);
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
                Zip = dto.ZipCode
            };

            if (dto.LogoFile != null)
            {
                var res = FileUtils.EncryptImage(dto.LogoFile);
                if (res.encryptedFile == null) return new GenericResponse<EmployerDto>(true, new HttpError("Logo file is in incorrect format", HttpStatusCode.BadRequest), null);

                e.Logo = res.encryptedFile;
            }

            var success = await _unitOfWork.Employers.Add(e);
            if(!success)
                return new GenericResponse<EmployerDto>(true, new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _unitOfWork.CompleteAsync();
            return new GenericResponse<EmployerDto>(false, null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericResponse<EmployerDto>> UpdateEmployer(int employerId, UpdateEmployerDto dto)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if(e == null) return new GenericResponse<EmployerDto>(true, new HttpError("Employer not found", HttpStatusCode.NotFound), null);

            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new GenericResponse<EmployerDto>(true, new HttpError("There is an employer with the same code and name combination", HttpStatusCode.BadRequest), null);
            }

            e.Name = dto.Name;
            e.Code = dto.Code;
            e.Email = dto.Email;
            e.Phone = dto.Phone;
            e.State = dto.State;
            e.Street = dto.Street;
            e.City = dto.City;
            e.Zip = dto.ZipCode;

            if (dto.newLogoFile != null)
            {
                var res = FileUtils.EncryptImage(dto.newLogoFile);
                if (res.encryptedFile == null) return new GenericResponse<EmployerDto>(true, new HttpError("Logo file is in incorrect format", HttpStatusCode.BadRequest), null);

                e.Logo = res.encryptedFile;
            }

            var success = _unitOfWork.Employers.Upsert(e);
            if (!success)
                return new GenericResponse<EmployerDto>(true, new HttpError("Server failed to save changes", HttpStatusCode.InternalServerError), null);

            await _unitOfWork.CompleteAsync();
            return new GenericResponse<EmployerDto>(false, null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericResponse<bool>> DeleteEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericResponse<bool>(true, new HttpError("Emplyer not found", HttpStatusCode.NotFound), false);

            var success = await _unitOfWork.Employers.Delete(e.Id);

            if (!success)
                return new GenericResponse<bool>(true, new HttpError("Server failed to delete employer", HttpStatusCode.InternalServerError), false);
            
            await _unitOfWork.CompleteAsync();
            return new GenericResponse<bool>(false, null, true);
        }

        public async Task<GenericResponse<EmployerDto>> GetEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new GenericResponse<EmployerDto>(true, new HttpError("Emplyer not found", HttpStatusCode.NotFound), null);

            return new GenericResponse<EmployerDto>(false, null, new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo));
        }

        public async Task<GenericResponse<EmployerDto[]>> GetAll()
        {
            var res = await _unitOfWork.Employers.All();

            if (res == null) return new GenericResponse<EmployerDto[]>(true, new HttpError("Server failed to fetch employers", HttpStatusCode.InternalServerError), null);

            return new GenericResponse<EmployerDto[]>(false, null, res.Select(e => new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo)).ToArray());
        }
    }
}
