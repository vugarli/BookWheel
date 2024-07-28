using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Auth
{
    public interface IAuthenticationService
    {
        public Task<bool> SendEmailConfirmationAsync(string email);
        public Task<bool> ConfirmEmailAsync(string token,string email);
        public Task ChangePasswordAsync(ChangePasswordDto dto);
        public Task<AuthResponse> LoginAsync(LoginDto dto);
        public Task<bool> RegisterAsync(RegisterDto dto);
    }
}
