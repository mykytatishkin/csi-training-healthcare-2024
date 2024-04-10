using CSI.IBTA.Customer.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.Customer.Extensions
{
    public static class ModelExtensions
    {
        public static LoginRequest ToDto(this LoginViewModel model)
        {
            return new LoginRequest(model.Username, model.Password);
        }
    }
}
