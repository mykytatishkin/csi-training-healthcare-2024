using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.IBTA.Shared.Entities
{
    public class EmployerUser
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public Employer Employer { get; set; } = null!;
    }
}
