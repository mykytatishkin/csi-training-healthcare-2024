using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackagePlanViewModel
    {
        public string ActionName { get; set; } = null!;
        public int? PlanId { get; set; }
        public int EmployerId { get; set; }
        public int PackageId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Contribution { get; set; }
        public int PlanTypeId { get; set; }
        public IList<PlanType> AvailablePlanTypes { get; set; } = new List<PlanType>();
    }
}
