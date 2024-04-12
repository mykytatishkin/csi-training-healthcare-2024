using CSI.IBTA.Shared.Authorization.Interfaces;
using CSI.IBTA.Shared.Constants;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Authorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CSI.IBTA.UserService.Authorization.Policies.Handlers
{
    public class EmployeeOwnerRequirementHandler : AuthorizationHandler<EmployeeOwnerRequirement, IEmployeeOwnedResource>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmployeeOwnerRequirement requirement,
            IEmployeeOwnedResource resourceEmployee)
        {
            var isAdmin = context.User.IsInRole(Role.Administrator.ToString());
            if (isAdmin) context.Succeed(requirement);

            var isEmployerAdmin = context.User.IsInRole(Role.EmployerAdmin.ToString());
            if (isEmployerAdmin && context.User.FindFirstValue(JwtTokenClaimConstants.EmployerId) == resourceEmployee.EmployerId.ToString()) 
            {
                context.Succeed(requirement);
            }

            var isConsumer = context.User.IsInRole(Role.Employee.ToString());
            if (isConsumer && context.User.FindFirstValue(JwtTokenClaimConstants.UserId) == resourceEmployee.EmployeeId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
