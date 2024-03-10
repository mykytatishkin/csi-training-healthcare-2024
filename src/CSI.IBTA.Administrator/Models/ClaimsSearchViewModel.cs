using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class ClaimsSearchViewModel
    {
        public IEnumerable<Claim> Claims { get; set; } = [];
        public IEnumerable<Employer> Employers { get; set; } = [];
        public string ClaimNumber { get; set; } = null!;
        public int EmployerId { get; set; }
    }
}
