namespace CSI.IBTA.Shared.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TypeId { get; set; }
        public PlanType PlanType { get; set; } = null!;
        public int EmployeeId { get; set; }
        public string Status { get; set; } = null!;
        public decimal Contribution { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; } = null!;
    }
}
