using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageUpdatePlanViewModel
    {
        public InsurancePackageModificationViewModel PackageModel { get; set; } = null!;
        public int EmployerId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Contribution { get; set; }
        public PlanTypeDto PlanType { get; set; }
    }
}