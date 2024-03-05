using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync()
        { 
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> CreateUserAsync(Guid Id)
        {
            throw new NotImplementedException();
        }


    }
}
