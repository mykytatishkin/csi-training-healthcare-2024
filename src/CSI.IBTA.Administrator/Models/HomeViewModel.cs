using CSI.IBTA.Shared.DataStructures;
using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Employer> Employers { get; set; }
        public CreateEmployerViewModel? CreateEmployerViewModel { get; set; }
    }
}
