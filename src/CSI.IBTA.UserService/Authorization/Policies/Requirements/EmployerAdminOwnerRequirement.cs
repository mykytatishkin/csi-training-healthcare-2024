using Microsoft.AspNetCore.Authorization;

namespace CSI.IBTA.UserService.Authorization.Policies.Requirements
{
    public record EmployerAdminOwnerRequirement : IAuthorizationRequirement;
    
}
