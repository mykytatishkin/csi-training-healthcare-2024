namespace CSI.IBTA.Shared.Entities
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateOnly PlanStart { get; set; }
        public DateOnly PlanEnd { get; set; }
        public string PayrollFrequency { get; set; } = null!;
    }
}
