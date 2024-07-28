using Azure.Core;
using BookWheel.Application.Auth;
using BookWheel.Application.Exceptions;
using BookWheel.Application.Services;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Interfaces;
using BookWheel.Infrastructure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Services
{
    public partial class AuthenticationService : Application.Auth.IAuthenticationService
    {
        public SignInManager<ApplicationIdentityUser> _signInManager { get; }
        public UserManager<ApplicationIdentityUser> _userManager { get; }
        public ICurrentUserService _currentUserService { get; }
        public ITokenService _tokenService { get; }
        public IEmailSender _emailSender { get; }
        public ApplicationDbContext _dbContext { get; }
        public IServiceProvider ServiceProvider { get; }

        public AuthenticationService(
            SignInManager<ApplicationIdentityUser> signInManager,
            UserManager<ApplicationIdentityUser> userManager,
            ICurrentUserService currentUserService,
            ITokenService tokenService,
            IEmailSender emailSender,
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _dbContext = dbContext;
            ServiceProvider = serviceProvider;
        }

        public async Task ValidateAndThrowException<T>(T dto)
        {
            using var scope = ServiceProvider.CreateScope();

            var context = new ValidationContext<T>(dto);

            var validators = scope.ServiceProvider.GetRequiredService<IEnumerable<IValidator<T>>>();


            var validationResults = await Task.WhenAll(validators
                .Select(v => v.ValidateAsync(context)));

            var errors = validationResults
                .Where(v => !v.IsValid)
                .SelectMany(res => res.Errors)
                .GroupBy(f => f.PropertyName);

            if (errors.Any())
            {
                var validationEx = new ModelValidationException();

                foreach (var error in errors)
                {
                    foreach (var errorDetail in error.ToList())
                        validationEx.UpsertDataList(error.Key, errorDetail.ErrorMessage);
                }
                throw validationEx;
            }
        }



        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
            await ValidateAndThrowException(loginDto);

            // check for customer and owner match with choice

            var applicationUser = await _userManager.FindByNameAsync(loginDto.Email);
            var roles = await _userManager.GetRolesAsync(applicationUser);

            if (roles.Contains("Customer") && loginDto.IsCustomer != true
                || roles.Contains("Owner") && loginDto.IsCustomer != false)
                return new AuthResponse() { Result = false };

            var response = new AuthResponse();

            var result = await _signInManager
                .PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                false, true);

            response.Result = result.Succeeded;
            response.IsLockedOut = result.IsLockedOut;
            response.IsNotAllowed = result.IsNotAllowed;
            response.RequiresTwoFactor = result.RequiresTwoFactor;
            response.Username = loginDto.Email;

            if (result.Succeeded)
            {
                response.Token = await _tokenService.GetTokenAsync(loginDto.Email);
            }

            return response;
        }

        public async Task<bool> ConfirmEmailAsync
            (
            string token,
            string email
            )
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user,token);

            return result.Succeeded;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            await ValidateAndThrowException(dto);

            var identityUser = new ApplicationIdentityUser();
            identityUser.Email = dto.Email;
            identityUser.UserName = dto.Email;
            identityUser.PhoneNumber = dto.PhoneNumber;
            IdentityResult result = await _userManager.CreateAsync(identityUser, dto.Password);

            if (result.Succeeded)
            { 
                ApplicationUserRoot user = null;

                if (dto.IsCustomer)
                {
                    user = new CustomerUserRoot(identityUser.Id, dto.Email, dto.Email, dto.Email, dto.PhoneNumber);
                    await _userManager.AddToRoleAsync(identityUser,"Customer");
                }
                else 
                {
                    user = new OwnerUserRoot(identityUser.Id, dto.Email, dto.Email, dto.Email, dto.PhoneNumber);
                    await _userManager.AddToRoleAsync(identityUser,"Owner");
                }
                await _dbContext.Set<ApplicationUserRoot>().AddAsync(user);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            
            await _emailSender.SendEmailAsync(identityUser.Email,"support@bookwheel.com","Email Confirmation",token);

            await _dbContext.SaveChangesAsync();
            }

            return result.Succeeded;
        }

        public async Task ChangePasswordAsync
            (
            ChangePasswordDto dto
            )
        {
            await ValidateAndThrowException(dto);

            var userId = _currentUserService.GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId);

            if(user is null)
                return;

            var result = await _userManager.ChangePasswordAsync
                (
                user,
                dto.OldPassword,
                dto.NewPassword
                );

            if (!result.Succeeded)
            {
                //TODO
                throw new Exception("Password change failed");
            }
        }

        public async Task<bool> SendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null && user.EmailConfirmed == false)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailSender.SendEmailAsync(email,"support@bookwheel.com","Email Confirmation",token);
                return true;
            }
            return false;
        }
    }
}
