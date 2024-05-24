using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookWheel.Api.Authorization.Requirements
{
    public class UserCustomerRequirement : IAuthorizationRequirement
    {
    }


    public class UserCustomerRequirementHandler : AuthorizationHandler<UserCustomerRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserCustomerRequirement requirement)
        {
            if(context.User is null)
                return Task.CompletedTask;  

            if(context.User.HasClaim(c=>c.Type == ClaimTypes.Role) && context.User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role).Value == "Customer")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
