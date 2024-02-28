﻿namespace CSI.IBTA.Shared.Entities
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime PlanStart { get; set; }
        public DateTime PlanEnd { get; set; }
        public string PayrollFrequency { get; set; } = null!;
    }
}
