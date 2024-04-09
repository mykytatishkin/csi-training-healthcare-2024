using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Models
{
    public class EmployeeViewModel
    {
        public FullEmployeeDto Employee { get; set; } = null!;
        public bool AllowAddConsumers { get; set; }
    }
}
