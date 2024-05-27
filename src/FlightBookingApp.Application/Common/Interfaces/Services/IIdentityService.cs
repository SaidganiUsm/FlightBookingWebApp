using FlightBookingApp.Application.Features.Auth.Responses.Login;
using FlightBookingApp.Application.Features.Auth.Responses.Register;
using FlightBookingApp.Application.Features.Auth.Responses.ResetPassword;

namespace FlightBookingApp.Application.Common.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<RegisterResponse> RegisterUserAsync(RegisterViewModel model);

        Task<RegisterResponse> ConfirmUserEmailAsync(string userId, string token);

        Task<ResetPasswordResponse> ForgetPasswordAsync(string email);

        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel model);

        Task<LoginResponse> LoginUserAsync(LoginViewModel userModel);
    }
}
