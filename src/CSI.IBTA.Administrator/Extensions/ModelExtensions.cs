using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared;

namespace CSI.IBTA.Administrator.Extensions
{
    public static class ModelExtensions
    {
        public static LoginRequest ToDto (this LoginViewModel model) 
        {
            return new LoginRequest(model.Username, model.Password); 
        }
    }
}
