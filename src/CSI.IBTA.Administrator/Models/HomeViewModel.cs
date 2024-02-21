using CSI.IBTA.Shared.Entities;

namespace CSI.IBTA.Administrator.Models
{
    public class HomeViewModel
    {
        public List<Employer> Employers { get; set; }
        public CreateEmployerViewModel? CreateEmployerViewModel { get; set; }
    }
}
