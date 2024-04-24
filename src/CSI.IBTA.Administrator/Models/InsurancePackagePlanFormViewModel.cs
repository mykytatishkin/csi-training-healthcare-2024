using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackagePlanFormViewModel
    {
        public int? SelectedPlanIndex { get; set; }
        public int PlanId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Contribution { get; set; }
        public PlanTypeDto PlanType { get; set; } = null!;
    }
}