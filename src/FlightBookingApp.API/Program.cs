using NLog;
using NLog.Web;
using FlightBookingApp.Application;
using FlightBookingApp.Infrastructure;
using FlightBookingApp.API.Services;
using FlightBookingApp.Application.Common.Interfaces.Services;
using Microsoft.OpenApi.Models;
using FlightBookingApp.Infrastructure.Persistence;
using System.Text.Json.Serialization;
using FlightBookingApp.API.Middlewares;

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

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Add services to the container.
            // To display enum values as strings in the response
            builder
                .Services.AddControllers()
                .AddJsonOptions(
                    options =>
                        options.JsonSerializerOptions.Converters.Add(
                            new JsonStringEnumConverter()
                        )
                );

            // So that the URLs are lowercase
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddRazorPages();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "FlightBookingWebApp",
                        Version = "v1",
                        Description =
                            "API for bookig tickets for flights",
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
                            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer a25sdfs9d5s\"",
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

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "CorsPolicy",
                    builder =>
                    {
                        builder
                            .WithOrigins("https://localhost:44452")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
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

            // So that the Swagger UI is available in production
            // To invoke the Swagger UI, go to https://<host>/swagger or https://<host>/swagger/index.html
            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auctionify v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseCustomExceptionHandler(); // Custom exception handler middleware

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();
            app.MapFallbackToFile("index.html");

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