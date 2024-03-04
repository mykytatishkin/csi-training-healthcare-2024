
namespace CSI.IBTA.Shared.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ClaimNumber { get; set; } = null!;
        public DateOnly DateOfService { get; set; }
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
    }
}
