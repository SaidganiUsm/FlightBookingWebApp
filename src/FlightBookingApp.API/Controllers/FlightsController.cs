using FlightBookingApp.Application.Common.Models.Requests;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Create;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Delete;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Update;
using FlightBookingApp.Application.Features.Flights.Admin.Query.GetAll;
using FlightBookingApp.Application.Features.Flights.Admin.Query.GetbyId;
using FlightBookingApp.Application.Features.Flights.User.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlightsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
        {
            var query = new GetAllFlightsQuery { PageRequest = pageRequest };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetFlightQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateFlightCommand command)
        {
            var flightId = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = flightId }, flightId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateFlightCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteFlightCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("available-flights")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetFlightForUsers([FromQuery] PageRequest pageRequest)
        {
            var query = new GetFlightsForUserQuery { PageRequest = pageRequest };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
