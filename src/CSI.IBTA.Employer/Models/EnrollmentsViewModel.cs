using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Models
{
    public class EnrollmentsViewModel
    {
        public List<FullEnrollmentDto> Enrollments { get; set; } = [];
        public List<FullInsurancePackageDto> Packages { get; set; } = [];
        public int EmployerId { get; set; }
        public int EmployeeId { get; set; }
    }
}
