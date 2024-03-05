using Microsoft.AspNetCore.Http;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Services
{
    public interface ICurrentUserService
    {
        public string GetCurrentUserId();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private HttpContext _context { get; set; }
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        public string GetCurrentUserId()
        {
            var userId = _context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Assert.IsTrue(!string.IsNullOrEmpty(userId), "User is null");

            return userId;
        }
    }
}
