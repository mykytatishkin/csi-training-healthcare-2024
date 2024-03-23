using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Models
{
    public class EmployeesSearchViewModel
    {
        public IEnumerable<EmployeeDto> Employees { get; set; } = [];
        public int EmployerId { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
