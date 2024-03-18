using BookWheel.Application.Auth;
using BookWheel.Application.Services;
using BookWheel.Infrastructure.Identity;
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
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly ITokenService _tokenClaimsService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(SignInManager<ApplicationIdentityUser> signInManager,
            ITokenService tokenClaimsService,
            UserManager<ApplicationIdentityUser> userManager, IAuthenticationService authenticationService)
        {
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var authResponse = await _authenticationService.LoginAsync(loginDto);
                return Ok(authResponse);
            }
            catch (AuthenticationValidationException validationException)
            {
                return BadRequest(validationException);
            }
            catch (Exception ex)
            {
                return BadRequest("Something bad happened, contact support!");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(
            [FromBody] RegisterDto registerDto)
        {
            try
            {
                await _authenticationService.RegisterAsync(registerDto);
                return Ok();
            }
            catch (AuthenticationValidationException validationException)
            {
                return BadRequest(validationException);
            }
            catch (Exception ex)
            {
                return BadRequest("Something bad happened, contact support!");
            }
        }

    }
}
