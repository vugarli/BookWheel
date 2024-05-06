using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookWheel.Api.Authorization.Requirements
{
    public class UserOwnerRequirement : IAuthorizationRequirement
    {
    }


    public class UserOwnerRequirementHandler : AuthorizationHandler<UserOwnerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOwnerRequirement requirement)
        {
            if (context.User is null)
                return Task.CompletedTask;

            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role) && context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "Owner")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
