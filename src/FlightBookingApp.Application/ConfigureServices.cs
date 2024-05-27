using FlightBookingApp.Application.Common.Options;
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

            return services;
        }
    }
}
