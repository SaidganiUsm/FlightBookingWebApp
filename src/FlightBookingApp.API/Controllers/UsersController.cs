using FlightBookingApp.Application.Features.Users.Command.Delete;
using FlightBookingApp.Application.Features.Users.Command.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> UpdateUserProfile(
            [FromForm] UpdateUserCommand updateUserCommand
        )
        {
            var result = await _mediator.Send(updateUserCommand);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteUser()
        {
            var result = await _mediator.Send(new DeleteUserCommand());

            return Ok(result);
        }
    }
}
