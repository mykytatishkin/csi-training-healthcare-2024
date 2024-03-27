using CSI.IBTA.Employer.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.DTOs.Login;

namespace CSI.IBTA.Employer.Extensions
{
    public static class ModelExtensions
    {
        public static LoginRequest ToDto(this LoginViewModel model)
        {
            return new LoginRequest(model.Username, model.Password);
        }
    }
}
