using CSI.IBTA.Shared.DataStructures;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models
{
    public class ClaimsSearchViewModel
    {
        //public PaginatedList<ViewClaimDto> Claims { get; set; } = null!; // todo remove this if everything works
        public IEnumerable<ViewClaimDto> Claims { get; set; } = [];
        public IEnumerable<EmployerDto> Employers { get; set; } = [];
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
