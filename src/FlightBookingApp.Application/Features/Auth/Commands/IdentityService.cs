using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Application.Common.Options;
using FlightBookingApp.Application.Features.Auth.Responses.Login;
using FlightBookingApp.Application.Features.Auth.Responses.Register;
using FlightBookingApp.Application.Features.Auth.Responses.Token;
using FlightBookingApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightBookingApp.Application.Features.Auth.Commands
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<IdentityService> _logger;
        private readonly AuthSettingsOptions _authSettingsOptions;

        public IdentityService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<IdentityService> logger,
            IOptions<AuthSettingsOptions> authSettingsOptions
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _authSettingsOptions = authSettingsOptions.Value;
        }

        public async Task<LoginResponse> LoginUserAsync(LoginViewModel userModel)
        {
            if (
                userModel is null
                || string.IsNullOrEmpty(userModel.Email)
                || string.IsNullOrEmpty(userModel.Password)
            )
            {
                return new LoginResponse
                {
                    Errors = new[] { "User data is emtpy" },
                    IsSuccess = false,
                };
            }
            var user = await _userManager.FindByEmailAsync(userModel.Email);

            if (user is null)
            {
                return new LoginResponse
                {
                    Errors = new[] { "User is not found" },
                    IsSuccess = false,
                };
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                userModel.Password,
                false,
                false
            );

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");
            }
            else
            {
                return new LoginResponse
                {
                    Errors = new[] { "Wrong password or email" },
                    IsSuccess = false,
                };
            }

            var token = await GenerateJWTTokenWithUserClaimsAsync(user);

            token.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!;
            token.UserId = user.Id;

            return new LoginResponse { IsSuccess = true, Result = token };
        }

        private async Task<TokenModel> GenerateJWTTokenWithUserClaimsAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email!), };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettingsOptions.Key));

            var token = new JwtSecurityToken(
                issuer: _authSettingsOptions.Issuer,
                audience: _authSettingsOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenModel { AccessToken = tokenAsString, ExpireDate = token.ValidTo };
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model is null)
                throw new NullReferenceException("Register Model is null")!;

            if (model.Password != model.ConfirmPassword)
                return new RegisterResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                 return new RegisterResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new RegisterResponse
            {
                Message = "User was not created",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }
    }
}
