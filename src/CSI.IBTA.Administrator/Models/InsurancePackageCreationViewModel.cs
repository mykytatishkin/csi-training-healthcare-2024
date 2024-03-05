using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageCreationViewModel
    {
        public int EmployerId { get; set; }
        public Package Package { get; set; } = null!;
        public List<Plan> Plans { get; set; } = null!;
        public int SelectedPlanTypeId { get; set; }
        public IList<PlanType> AvailablePlanTypes { get; set; } = new List<PlanType>();
    }
}
