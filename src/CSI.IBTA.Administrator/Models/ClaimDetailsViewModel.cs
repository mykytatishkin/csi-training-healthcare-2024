using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class ClaimDetailsViewModel
    {
        public ClaimDto Claim { get; set; } = null!;
        public UserDto Consumer { get; set; } = null!;
        public decimal EnrollmentBalance { get; set; }
    }
}
