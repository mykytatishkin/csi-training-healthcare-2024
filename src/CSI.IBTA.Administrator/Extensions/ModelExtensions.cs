using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.Administrator.Extensions
{
    public static class ModelExtensions
    {
        public static LoginRequest ToDto (this LoginViewModel model) 
        {
            return new LoginRequest(model.Username, model.Password); 
        }

        public static CreateEmployerDto ToDto(this CreateEmployerViewModel model)
        {
            return new CreateEmployerDto(model.Name, model.Code, model.Email, model.Street, model.City, model.State, model.ZipCode, model.Phone, model.Logo);
        }
    }
}
