using BookWheel.Domain.AggregateRoots;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Identity
{
    public class TokenService
    {

        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public TokenService
            (
            UserManager<ApplicationIdentityUser> userManager,
            ApplicationDbContext dbContext
            )
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<string> GetTokenAsync(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(AppConstants.JWTKEY);
            var applicationUser = await _userManager.FindByNameAsync(userName);

            //if (applicationUser == null) throw new UserNotFoundException(userName);

            var user = _dbContext.Set<ApplicationUserRoot>()
                .FirstOrDefault(u => u.Id == applicationUser.Id);

            var roles = await _userManager.GetRolesAsync(applicationUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, applicationUser.Email),
                new Claim(ClaimTypes.Name, applicationUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
