using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageFormViewModel
    {
        public int EmployerId { get; set; }
        public FullInsurancePackageDto Package { get; set; } = null!;
        public List<PlanTypeDto> AvailablePlanTypes { get; set; } = new List<PlanTypeDto>();
        public InsurancePackagePlanFormViewModel PlanForm { get; set; }
    }
}