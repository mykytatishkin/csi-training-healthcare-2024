﻿using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageCreationViewModel
    {
        public int EmployerId { get; set; }
        public CreateInsurancePackageDto Package { get; set; } = null!;
        public List<PlanDto> Plans { get; set; } = null!;
        public int SelectedPlanTypeId { get; set; }
        public IList<PlanTypeDto> AvailablePlanTypes { get; set; } = new List<PlanTypeDto>();
        public PlanDto? SelectedPlan { get; set; }
    }
}