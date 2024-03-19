using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageModificationViewModel
    {
        public int EmployerId { get; set; }
        public FullInsurancePackageDto Package { get; set; } = null!;
        public IList<PlanDto> Plans { get; set; } = null!;
        public int SelectedPlanTypeId { get; set; }
        public IList<PlanTypeDto> AvailablePlanTypes { get; set; } = new List<PlanTypeDto>();
        public int SelectedPlanIndex { get; set;  }
    }
}