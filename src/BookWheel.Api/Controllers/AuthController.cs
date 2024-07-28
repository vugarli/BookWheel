using BookWheel.Application.Auth;
using BookWheel.Application.Services;
using BookWheel.Domain.Interfaces;
using BookWheel.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUserService currentUserService;

        public IEmailSender _emailSender { get; }

        public AuthController(
            SignInManager<ApplicationIdentityUser> signInManager,
            UserManager<ApplicationIdentityUser> userManager,
            IAuthenticationService authenticationService,
            ICurrentUserService currentUserService,
            IEmailSender emailSender
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authenticationService = authenticationService;
            this.currentUserService = currentUserService;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto loginDto)
        {
            var authResponse = await _authenticationService.LoginAsync(loginDto);
            
            if (!authResponse.Result)
                return Unauthorized();

            return Ok(authResponse);
        }

        [HttpGet("sendconfirmation")]
        public async Task<ActionResult> ChangePasswordAsync
            (
                [FromQuery] string email
            )
        {
            var result = await _authenticationService.SendEmailConfirmationAsync(email);
            return Ok();
        }

        [HttpGet("confirmemail")]
        public async Task<ActionResult> ChangePasswordAsync
            (
                [FromQuery] string token,
                [FromQuery] string email
            )
        {
            var result = await _authenticationService.ConfirmEmailAsync(token,email);
            
            if (!result)
                return BadRequest();
            
            return Ok();
        }


        [HttpPost("changepassword")]
        [Authorize()]
        public async Task<ActionResult> ChangePasswordAsync
            (
                [FromBody] ChangePasswordDto changePasswordDto
            )
        {
            await _authenticationService.ChangePasswordAsync(changePasswordDto);
            return Ok();
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse),StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResponse>> Register
            (
            [FromBody] RegisterDto registerDto
            )
        {
            var result = await _authenticationService.RegisterAsync(registerDto);
            
            if(!result)
                return BadRequest();
            
            return Ok();
         }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("info")]
        public IActionResult GetUserInfo()
        { 
            var userId = currentUserService.GetCurrentUserId();
            return Ok(userId);
        }

        [HttpGet("email")]
        public async Task<IActionResult> Email()
        {
            await _emailSender.SendEmailAsync("a@a.com","b@B.com","aa","aa");
            return Ok();
        }



    }
}
