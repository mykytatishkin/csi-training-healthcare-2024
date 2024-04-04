using CSI.IBTA.Shared.Constants;
using CSI.IBTA.Shared.Entities;
using CSI.IBTA.UserService.Authorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CSI.IBTA.UserService.Authorization.Policies.Handlers
{
    public class EmployerAdminOwnerRequirementHandler : AuthorizationHandler<EmployerAdminOwnerRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmployerAdminOwnerRequirement requirement,
            int resourceEmployerId)
        {
            var isAdmin = context.User.IsInRole(Role.Administrator.ToString());
            if (isAdmin) context.Succeed(requirement);

            var isEmployerAdmin = context.User.IsInRole(Role.EmployerAdmin.ToString());
            if (!isEmployerAdmin) return Task.CompletedTask;

            var employerId = context.User.FindFirstValue(JwtTokenClaimConstants.EmployerId);
            if(employerId == resourceEmployerId.ToString()) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
