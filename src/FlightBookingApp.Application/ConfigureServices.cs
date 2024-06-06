using FlightBookingApp.Application.Common.Behaviours;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Application.Common.Options;
using FlightBookingApp.Application.Features.Auth.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlightBookingApp.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<AppOptions>(configuration.GetSection(AppOptions.App));
            services.Configure<AuthSettingsOptions>(configuration.GetSection(AuthSettingsOptions.AuthSettings));
            
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
