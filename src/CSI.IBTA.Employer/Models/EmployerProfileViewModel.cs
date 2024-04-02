using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Models
{
    public class EmployerProfileViewModel
    {
        public EmployerDto Employer { get; set; }
        public IFormFile? NewLogo { get; set; }
    }
}
