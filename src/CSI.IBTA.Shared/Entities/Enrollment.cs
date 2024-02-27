using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.IBTA.Shared.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public decimal Election { get; set; }
        public int EmployeeId { get; set; }
        public int PlanId { get; set; }
    }
}
