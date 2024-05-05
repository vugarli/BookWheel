using BookWheel.Application.Auth;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Services
{
    public partial class AuthenticationService : Application.Auth.IAuthenticationService
    {
        public SignInManager<ApplicationIdentityUser> _signInManager { get; }
        public UserManager<ApplicationIdentityUser> _userManager { get; }
        public ITokenService _tokenService { get; }
        public ApplicationDbContext _dbContext { get; }

        public AuthenticationService(
            SignInManager<ApplicationIdentityUser> signInManager,
            UserManager<ApplicationIdentityUser> userManager,
            
            ITokenService tokenService,
            ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
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

        public async Task RegisterAsync(RegisterDto dto)
        {

            var identityUser = new ApplicationIdentityUser();
            identityUser.Email = dto.Email;
            identityUser.UserName = dto.Email;
            IdentityResult result=new();
            try
            {
                result = await _userManager.CreateAsync(identityUser, dto.Password);
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }


            if (result.Succeeded)
            { 
                ApplicationUserRoot user = null;

                if (dto.IsCustomer)
                {
                    user = new CustomerUserRoot(identityUser.Id, dto.Email, dto.Email, dto.Email);
                    await _userManager.AddToRoleAsync(identityUser,"Customer");
                }
                else 
                {
                    user = new OwnerUserRoot(identityUser.Id, dto.Email, dto.Email, dto.Email);
                    await _userManager.AddToRoleAsync(identityUser,"Owner");
                }
                await _dbContext.Set<ApplicationUserRoot>().AddAsync(user);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
