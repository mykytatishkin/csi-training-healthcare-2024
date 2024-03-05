using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageNewPlanViewModel
    {
        public InsurancePackageCreationViewModel PackageModel { get; set; } = null!;
        public int EmployerId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Contribution { get; set; }
        public PlanType PlanType { get; set; }
    }
}
