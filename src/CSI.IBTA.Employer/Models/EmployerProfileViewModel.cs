using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Models
{
    public class EmployerProfileViewModel
    {
        public EmployerWithConsumerSettingDto Employer { get; set; } = null!;
        public IFormFile? NewLogo { get; set; }
    }
}
