namespace CSI.IBTA.Shared.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public decimal Election { get; set; }
        public int EmployeeId { get; set; }
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
    }
}
