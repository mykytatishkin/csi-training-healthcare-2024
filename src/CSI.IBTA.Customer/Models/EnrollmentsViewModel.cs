using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Customer.Models
{
    public class EnrollmentsViewModel
    {
        public IEnumerable<FullEnrollmentDto> Enrollments { get; set; } = [];
        public int EmployerId { get; set; }
        public int EmployeeId { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
