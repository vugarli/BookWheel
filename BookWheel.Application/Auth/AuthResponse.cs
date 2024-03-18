using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Auth
{
    public class AuthResponse
    {
        public AuthResponse()
        {
        }
        public bool Result { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsLockedOut { get; set; } = false;
        public bool IsNotAllowed { get; set; } = false;
        public bool RequiresTwoFactor { get; set; } = false;
    }
}
