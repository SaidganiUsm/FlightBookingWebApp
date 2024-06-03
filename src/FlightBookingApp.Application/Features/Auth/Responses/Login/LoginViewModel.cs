using System.ComponentModel.DataAnnotations;

namespace FlightBookingApp.Application.Features.Auth.Responses.Login
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
