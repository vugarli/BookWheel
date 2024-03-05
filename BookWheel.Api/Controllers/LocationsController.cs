using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> SetLocationAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLocation()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}/ratings")]
        public async Task<IActionResult> GetRatingsAsync()
        {
            throw new NotImplementedException();
        }



    }
}
