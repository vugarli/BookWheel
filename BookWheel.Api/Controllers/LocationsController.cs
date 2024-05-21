using BookWheel.Api.CustomAttribute;
using BookWheel.Application.Locations.Commands;
using BookWheel.Application.Locations.Queries;
using BookWheel.Application.LocationServices.Commands;
using BookWheel.Application.LocationServices.Queries;
using BookWheel.Application.Reservations.Commands;
using BookWheel.Application.Reservations.Queries;
using BookWheel.Domain.Exceptions;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System.Net.Mime;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : RESTFulController
    {
        private IMediator _mediator { get; }

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy ="CanSetLocation")]
        public async Task<IActionResult> SetLocationAsync
            (
            SetLocationCommand setLocationCommand
            )
        {
            await _mediator.Send(setLocationCommand);            
            return Ok();
        }

        [HttpPatch("{LocationId:guid}/boxnumber")]
        [Authorize(Policy = "Owner")]
        public async Task<IActionResult> UpdateBoxNumberAsync
            (
            [FromHybrid] UpdateBoxNumberCommand updateBoxNumberCommand
            )
        {
            await _mediator.Send(updateBoxNumberCommand);
            return Ok();
        }

        [HttpPatch("{LocationId:guid}/coordinates")]
        [Authorize(Policy = "Owner")]
        public async Task<IActionResult> CloseLocationAsync
            (
            [FromHybrid] UpdateCoordinatesCommand updateCoordinatesCommand
            )
        {
            await _mediator.Send(updateCoordinatesCommand);
            return Ok();
        }

        [HttpPatch("{LocationId:guid}/workingtime")]
        [Authorize(Policy = "Owner")]
        public async Task<IActionResult> CloseLocationAsync
            (
            [FromHybrid] UpdateWorkingTimeCommand updateWorkingTimeCommand
            )
        {
            await _mediator.Send(updateWorkingTimeCommand);
            return Ok();
        }

        [HttpPost("{LocationId:guid}/close")]
        [Authorize(Policy ="Owner")]
        public async Task<IActionResult> CloseLocationAsync
            (
            [FromRoute] CloseLocationCommand closeLocationCommand
            )
        {
            await _mediator.Send(closeLocationCommand);
            return Ok();
        }

        [HttpPost("{LocationId:guid}/open")]
        [Authorize(Policy = "Owner")]
        public async Task<IActionResult> OpenLocationAsync
            (
            [FromRoute] OpenLocationCommand openLocationCommand
            )
        {
            await _mediator.Send(openLocationCommand);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            return Ok(await _mediator.Send(new GetAllLocationsQuery()));
        }

        [HttpGet("{locationId:guid}/services")]
        public async Task<IActionResult> GetAllLocationServicesAsync(Guid locationId)
        {
            return Ok(await _mediator.Send(new GetAllLocationServicesQuery(locationId)));
        }

        [HttpPost("{LocationId:guid}/services")]
        public async Task<IActionResult> AddLocationServiceAsync
            (
            [FromHybrid] AddServiceCommand command 
            )
        {
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy ="Owner")]
        [HttpDelete("{LocationId:guid}/services/{ServiceId:guid}")]
        public async Task<IActionResult> DeleteLocationServiceAsync
            (
            [FromHybrid] DeleteServiceCommand command
            )
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{locationId:guid}/timeslots")]
        public async Task<IActionResult> GetTimeSlotsForLocation(Guid locationId)
        {
            return Ok(await _mediator.Send(new GetLocationTimeSlotsQuery(locationId)));
        }

        [HttpGet("{locationId:guid}")]
        public async Task<IActionResult> GetLocationAsync(Guid locationId)
        {
            return Ok(await _mediator.Send(new GetLocationQuery(locationId)));
        }

        [HttpGet("{locationId:guid}/ratings")]
        public async Task<IActionResult> GetRatingsAsync(Guid locationId)
        {
            return Ok(await _mediator.Send(new GetLocationRatingsQuery(locationId)));
        }

        [HttpGet("{locationId:guid}/reservations")]
        public async Task<IActionResult> PostReservationAsync
            (
            Guid locationId
            )
        {
            return Ok(await _mediator.Send(new GetLocationReservationsQuery(locationId)));
        }

        [HttpPost("{LocationId:guid}/reservations")]
        [Authorize(Policy = "CanReserve")]
        public async Task<IActionResult> PostReservationAsync
            (
            [FromHybrid] CreateReservationCommand command
            )
        {
            await _mediator.Send(command);
            return Ok();
        }
        
        [Authorize(Policy = "Owner")]
        [HttpDelete("{LocationId:guid}/reservations/{ReservationId:guid}")]
        public async Task<IActionResult> DeleteReservationAsync
            (
            [FromRoute] CancelReservationByOwnerCommand command
            )
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
