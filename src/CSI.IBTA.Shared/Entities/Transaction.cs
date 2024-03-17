namespace CSI.IBTA.Shared.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime DateTime { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public int EnrollmentId { get; set; }
    }
}
