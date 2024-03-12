using BookWheel.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IUserService UserService { get; }

        public AuthController(IUserService userService)
        {
            UserService = userService;
        }

        public async Task<Results<Ok, ValidationProblem>> RegisterAsync
            (
            [FromBody] RegisterRequest registration,
            [FromServices] IServiceProvider sp
            )
        {
            var userManager = sp.GetRequiredService<UserManager<IdentityUser<Guid>>>();

            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"Requires a user store with email support.");
            }

            var userStore = sp.GetRequiredService<IUserStore<IdentityUser>>();
            var emailStore = (IUserEmailStore<IdentityUser>)userStore;
            var email = registration.Email;            

            var user = new IdentityUser<Guid>();
            
            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }
            await UserService.CreateOwnerUserAsync(user.Id,user.Email);
            //await SendConfirmationEmailAsync(user, userManager, context, email);
            return TypedResults.Ok();
        }




        [NonAction]
        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            // We expect a single error code and description in the normal case.
            // This could be golfed with GroupBy and ToDictionary, but perf! :P
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }
    }
}
