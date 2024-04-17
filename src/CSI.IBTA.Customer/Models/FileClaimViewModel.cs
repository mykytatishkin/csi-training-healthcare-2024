using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Models
{
    public class FileClaimViewModel
    {
        public int EmployeeId { get; set; }
        public int EmployerId {  get; set; }
        public IEnumerable<FullEnrollmentWithBalanceDto> Enrollments { get; set; } = [];
        public DateOnly DateOfService {  get; set; }
        public int EnrollmentId {  get; set; }
        public decimal Amount { get; set; }
        public IFormFile Receipt { get; set; }
    }
}
