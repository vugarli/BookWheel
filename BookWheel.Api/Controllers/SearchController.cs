using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        [HttpGet("{query:string}")]
        public async Task<IActionResult> SearchAsync()
        {
            throw new NotImplementedException();
        }

    }
}
