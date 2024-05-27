using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Application.Features.Auth.Responses.Login;
using FlightBookingApp.Application.Features.Auth.Responses.Register;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(
            IIdentityService identityService,
            ICurrentUserService currentUserService
        )
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid.");
            }

            var result = await _identityService.RegisterUserAsync(model);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var result = await _identityService.LoginUserAsync(loginModel);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
