using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Auth
{
    public interface IAuthenticationService
    {
        public Task<AuthResponse> LoginAsync(LoginDto dto);
        public Task RegisterAsync(RegisterDto dto);
    }
}
