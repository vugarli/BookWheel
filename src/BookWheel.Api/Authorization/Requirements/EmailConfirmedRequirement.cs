using Microsoft.AspNetCore.Authorization;

namespace BookWheel.Api.Authorization.Requirements
{
    public class EmailConfirmedRequirement : IAuthorizationRequirement
    {
    }

    public class EmailConfirmedRequirementHandler : AuthorizationHandler<EmailConfirmedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailConfirmedRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;

            if(context.User is null)
                return Task.CompletedTask;

            if (context.User.HasClaim(c => c.Type == "EmailConfirmed"))
                context.Succeed(requirement);

            context.Fail();
            return Task.CompletedTask;
        }
    }


}
