using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedulesCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> CancelSchedule()
        {
            throw new NotImplementedException();
        }

    }
}
