using Initialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Core;
using HealthChecks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

/********************************************************
DO NOT EDIT THIS FILE. IF A CHANGE IS NEEDED UPDATE THE
TEMPLATE AND THEN RE-APPLY
*********************************************************/

// Create our webapplication builder
WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Configure Serilog as the logger
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Logger logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

Log.Logger = logger;
builder.WebHost.UseUrls("http://*:8080");

// Add services to the container.
builder.Host.ConfigureServices((_, services) =>
{
    var healthCheck = services.AddHealthChecks();
    healthCheck.AddCheck<VersionHealthCheck>(VersionHealthCheck.NAME);
    Service.ConfigureServices(_, services, healthCheck);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Setup Serilog request logging
app.UseSerilogRequestLogging((options) =>
{
    options.Logger = logger;
});



app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    AllowCachingResponses = false,
    ResponseWriter = Service.WriteHealthCheckResponse
});


// Tells caller we are alive
app.MapGet("/liveness", () => "Alive")
.WithName("liveness")
.WithDisplayName("Liveness Check")
.WithDescription("Whether the service is hung")
.WithTags("k8s")
.WithGroupName("k8s")
.Produces<string>(StatusCodes.Status200OK);

Service.MapServiceEndpoints(app);

app.Run();

public partial class Program
{ }