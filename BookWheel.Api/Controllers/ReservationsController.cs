using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserReservationsAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReservationByIdAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> CancelReservationAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:guid}/ratings")]
        public async Task<IActionResult> RateReservationAsync()
        {
            throw new NotImplementedException();
        }

    }
}
