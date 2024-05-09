using BookWheel.Application.Ratings.Commands;
using BookWheel.Application.Reservations.Queries;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public IMediator Mediator { get; }
        public ReservationsController(IMediator mediator)
        {
            Mediator = mediator;
        }


        [HttpGet]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> GetCurrentUserReservationsAsync()
        {
            return Ok(await Mediator.Send(new GetCurrentCustomerReservationsQuery()));
        }

        [HttpGet("{ReservationId:guid}")]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> GetReservationByIdAsync
            (
            [FromHybrid] GetCurrentCustomerReservationByIdQuery query
            )
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPut("{ReservationId:guid}/ratings")]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> RateReservationAsync
            (
            [FromHybrid] RateReservationCommand command
            )
        {
            await Mediator.Send(command);
            return Ok();
        }

    }
}
