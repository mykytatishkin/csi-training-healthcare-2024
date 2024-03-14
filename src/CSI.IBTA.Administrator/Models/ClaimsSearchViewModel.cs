using CSI.IBTA.Shared.DataStructures;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class ClaimsSearchViewModel
    {
        public PaginatedList<ViewClaimDto> Claims { get; set; } = null!;
        public IEnumerable<EmployerDto> Employers { get; set; } = [];
    }
}
