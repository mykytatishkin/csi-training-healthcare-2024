﻿
using CSI.IBTA.Shared.Utils.Extensions;

namespace CSI.IBTA.Shared.Entities
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime PlanStart { get; set; }
        public DateTime PlanEnd { get; set; }
        public bool IsRemoved { get; set; } = false;

        public bool IsActive
        {
            get
            {
                if (Initialized == null) return false;
                var now = DateTime.UtcNow;
                return now > PlanStart && now < PlanEnd;
            }
        }

        public string Status
        {
            get
            {
                var now = DateTime.UtcNow;
                if (now > PlanEnd) return $"Archived on {PlanEnd.ToAmericanDateOnlyFormat()}";
                if (Initialized == null) return "Not Initialized";

                return $"Initialized on {Initialized.ToAmericanDateOnlyFormat()}";
            }
        }

        public PayrollFrequency PayrollFrequency { get; set; }
        public DateOnly? Initialized { get; set; }
        public int EmployerId { get; set; }
        public IList<Plan> Plans { get; set; } = new List<Plan>();
    }
}