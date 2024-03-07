using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class InsurancePackageViewModel
    {
        public int EmployerId { get; set; }
        public List<InsurancePackageDto> InsurancePackages { get; set; } = [];
    }
}
