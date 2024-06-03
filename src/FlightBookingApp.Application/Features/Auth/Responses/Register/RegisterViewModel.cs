﻿using System.ComponentModel.DataAnnotations;

namespace FlightBookingApp.Application.Features.Auth.Responses.Register
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(254, ErrorMessage = "Email is too long.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
