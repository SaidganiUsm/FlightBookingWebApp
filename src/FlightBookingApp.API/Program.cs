using NLog;
using NLog.Web;
using FlightBookingApp.Application;
using FlightBookingApp.Infrastructure;
using FlightBookingApp.API.Services;
using FlightBookingApp.Application.Common.Interfaces.Services;
using Microsoft.OpenApi.Models;
using FlightBookingApp.Infrastructure.Persistence;

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
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "FlightBookingApp",
                        Version = "v1",
                        Description =
                            "API for FlightBookingApp to manage tickets to flights.",
                    }
                );

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                    }
                );
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using var scope = app.Services.CreateScope();
                var initialiser =
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
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