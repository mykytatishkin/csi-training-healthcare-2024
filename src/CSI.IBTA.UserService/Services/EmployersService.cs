using CSI.IBTA.DataLayer.Interfaces;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.UserService.Types;
using CSI.IBTA.UserService.Utils;

namespace CSI.IBTA.UserService.Services
{
    internal class EmployersService : IEmployersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<EmployerDto?>> CreateEmployer(CreateEmployerDto dto)
        {
            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new ServiceResponse<EmployerDto?>(null, "There is an employer with the same code and name combination");
            }

            var description = "Employer created successfully";

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
                if (res.encryptedFile == null) description = res.description;

                e.Logo = res.encryptedFile;
            }

            var success = await _unitOfWork.Employers.Add(e);
            if(!success)
                return new ServiceResponse<EmployerDto?>(null, "Server failed to save changes");

            await _unitOfWork.CompleteAsync();
            return new ServiceResponse<EmployerDto?>(new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo), description);
        }

        public async Task<ServiceResponse<EmployerDto?>> UpdateEmployer(int employerId, UpdateEmployerDto dto)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if(e == null) return new ServiceResponse<EmployerDto?>(null, "Employer not found");

            var hasSameCombination = await _unitOfWork.Employers
                .Find(x => x.Name == dto.Name && x.Code == dto.Code);

            if (hasSameCombination.Any())
            {
                return new ServiceResponse<EmployerDto?>(null, "There is an employer with the same code and name combination");
            }

            var description = "Employer created successfully";

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
                if (res.encryptedFile == null) description = res.description;

                e.Logo = res.encryptedFile;
            }

            var success = _unitOfWork.Employers.Upsert(e);
            if (!success)
                return new ServiceResponse<EmployerDto?>(null, "Server failed to save changes");

            await _unitOfWork.CompleteAsync();
            return new ServiceResponse<EmployerDto?>(new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo), description);
        }

        public async Task<ServiceResponse<bool>> DeleteEmployer(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new ServiceResponse<bool>(false, "Employer not found");
            
            var success = await _unitOfWork.Employers.Delete(e.Id);

            if (!success)
                return new ServiceResponse<bool>(false, "Server failed to delete employer");
            
            await _unitOfWork.CompleteAsync();
            return new ServiceResponse<bool>(true, "Employer was successfullly deleted");
        }

        public async Task<ServiceResponse<EmployerDto?>> GetEmployerProfile(int employerId)
        {
            var e = await _unitOfWork.Employers.GetById(employerId);

            if (e == null) return new ServiceResponse<EmployerDto?>(null, "Employer not found");

            return new ServiceResponse<EmployerDto?>(new EmployerDto(e.Id, e.Name, e.Code, e.Email, e.Street, e.City, e.State, e.Zip, e.Phone, e.Logo), "Employer profile information was successfully returned");
        }
    }
}
