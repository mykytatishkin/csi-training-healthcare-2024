using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class ClaimDetailsViewModel
    {
        public ClaimDto Claim { get; set; }
        public UserDto Consumer { get; set; }
        public decimal EnrollmentBalance { get; set; }
    }
}
