using NLog;
using NLog.Web;
using FlightBookingApp.Application;
using FlightBookingApp.Infrastructure;
using FlightBookingApp.API.Services;
using FlightBookingApp.Application.Common.Interfaces.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var logger = NLog.LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

        try
        {
            logger.Debug("init main");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (HostAbortedException ex)
        {
            logger.Info("Ignore HostAbortedException", ex.Message);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception.");
            throw;
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }
    }
}