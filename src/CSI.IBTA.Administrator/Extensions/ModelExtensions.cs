using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.Administrator.Extensions
{
    public static class ModelExtensions
    {
        public static LoginRequest ToDto(this LoginViewModel model)
        {
            return new LoginRequest(model.Username, model.Password);
        }

        public static CreateEmployerDto ToCreateEmployerDto(this EmployerFormViewModel model)
        {
            return new CreateEmployerDto(model.Name, model.Code, model.Email, model.Street, model.City, model.State, model.ZipCode, model.Phone, model.NewLogo);
        }

        public static UpdateEmployerDto ToUpdateEmployerDto(this EmployerFormViewModel model)
        {
            return new UpdateEmployerDto(model.Name, model.Code, model.Email, model.Street, model.City, model.State, model.ZipCode, model.Phone, model.NewLogo);
        }

        public static EmployerFormViewModel ToFormViewModel(this EmployerDto dto)
        {
            return new EmployerFormViewModel()
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code,
                Email = dto.Email,
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Phone = dto.Phone,
                EncodedLogo = dto.EncodedLogo
            };
        }
    }
}
