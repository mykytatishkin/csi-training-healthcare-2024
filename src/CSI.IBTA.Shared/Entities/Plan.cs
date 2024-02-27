using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.IBTA.Shared.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; } = null!;
        public decimal Contribution { get; set; }
        public int PackageId { get; set; }
    }
}
