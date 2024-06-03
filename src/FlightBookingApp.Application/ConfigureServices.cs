using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Application.Common.Options;
using FlightBookingApp.Application.Features.Auth.Commands;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightBookingApp.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<AppOptions>(configuration.GetSection(AppOptions.App));
            services.Configure<AuthSettingsOptions>(configuration.GetSection(AuthSettingsOptions.AuthSettings));

            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
