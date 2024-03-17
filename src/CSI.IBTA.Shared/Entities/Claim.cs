
namespace CSI.IBTA.Shared.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public string ClaimNumber { get; set; } = null!;
        public DateOnly DateOfService { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public decimal Amount { get; set; }
        public ClaimStatus Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}
