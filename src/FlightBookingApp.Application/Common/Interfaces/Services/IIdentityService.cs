using FlightBookingApp.Application.Features.Auth.Responses.Login;
using FlightBookingApp.Application.Features.Auth.Responses.Register;

namespace FlightBookingApp.Application.Common.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<RegisterResponse> RegisterUserAsync(RegisterViewModel model);

        Task<LoginResponse> LoginUserAsync(LoginViewModel userModel);
    }
}
