using CSI.IBTA.Shared.Authorization.Types;

namespace CSI.IBTA.UserService.Authorization.Extensions
{
    public static class EmployeeOwnedResourceExtensions
    {
        public static EmployeeOwnedResource GetResource(this (int employeeId, int employerId) data) 
        {
            return new EmployeeOwnedResource()
            {
                EmployeeId = data.employeeId,
                EmployerId = data.employerId,
            };
        }
    }
}
