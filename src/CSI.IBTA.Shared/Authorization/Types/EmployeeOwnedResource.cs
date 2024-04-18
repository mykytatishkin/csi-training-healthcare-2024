using CSI.IBTA.Shared.Authorization.Interfaces;

namespace CSI.IBTA.Shared.Authorization.Types
{
    public class EmployeeOwnedResource : IEmployeeOwnedResource
    {
        public int EmployeeId { get; set; }
        public int EmployerId { get; set; }
    }
}
