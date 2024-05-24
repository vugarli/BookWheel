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
            //try
            //{
                var authResponse = await _authenticationService.LoginAsync(loginDto);
                return Ok(authResponse);
            //}
            //catch (AuthenticationValidationException validationException)
            //{
            //    return BadRequest(validationException);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Something bad happened, contact support!");
            //}
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register
            (
            [FromBody] RegisterDto registerDto
            )
        {
            //try
            //{
                await _authenticationService.RegisterAsync(registerDto);
                return Ok();
            //}
            //catch (AuthenticationValidationException validationException)
            //{
            //    return BadRequest(validationException);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Something bad happened, contact support!");
            //}
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
