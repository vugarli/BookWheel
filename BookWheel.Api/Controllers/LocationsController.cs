﻿using BookWheel.Application.Locations.Commands;
using BookWheel.Application.Locations.Queries;
using BookWheel.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

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
        public async Task<IActionResult> SetLocationAsync
            (
            SetLocationCommand setLocationCommand
            )
        {
            try
            {
                await _mediator.Send(setLocationCommand);
            }
            catch(OwnerAlreadyHasLocationSet ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            return Ok(await _mediator.Send(new GetAllLocationsQuery()));
        }

        [HttpGet("{locationId:guid}/services")]
        public async Task<IActionResult> GetAllLocations(Guid locationId)
        {
            return Ok(await _mediator.Send(new GetAllLocationServicesQuery(locationId)));
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

        [HttpPost("{id:guid}/reservations")]
        public async Task<IActionResult> PostReservationAsync()
        {
            throw new NotImplementedException();
        }


    }
}
