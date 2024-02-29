using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageCreationViewModel
    {
        public Package Package { get; set; } = null!;
        public List<Plan> Plans { get; set; } = null!;
    }
}
