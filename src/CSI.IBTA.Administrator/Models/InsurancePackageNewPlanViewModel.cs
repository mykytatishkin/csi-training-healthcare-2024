using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageNewPlanViewModel
    {
        public InsurancePackageCreationViewModel PackageModel { get; set; } = null!;
        public int EmployerId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Contribution { get; set; }
        public int PlanTypeId { get; set; }
        public IList<PlanType> AvailablePlanTypes { get; set; } = new List<PlanType>();
    }
}
