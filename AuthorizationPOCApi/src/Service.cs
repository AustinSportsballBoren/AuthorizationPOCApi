using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks;
using OpenFGA;

namespace Initialization;

internal class Service
{
    /// <summary>
    /// Logger
    /// </summary>
    private ILogger<Service> _log;

    public Service(ILogger<Service> log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }


    /// <summary>
    /// Register additional services in the dependency injection system.
    /// </summary>
    /// <param name="hbContext"></param>
    /// <param name="services">Service collection to add services to</param>
    /// <param name="healthChecks">Called to add health checks</param>
    internal static void ConfigureServices(HostBuilderContext hbContext, IServiceCollection services, IHealthChecksBuilder? healthChecks)
    {
        OpenFGA.OpenFGA openFGA = new OpenFGA.OpenFGA("http://localhost:8080", "");
        services.AddSingleton<IOpenFGA>(openFGA);
    }

    /// <summary>
    /// Map service endpoints
    /// </summary>
    /// <param name="app"></param>
    internal static void MapServiceEndpoints(WebApplication app)
    {
        var openfga = app.MapGroup("openfga");
        openfga.MapOpenFGAEndpoints();

        var spicedb = app.MapGroup("spicedb");
        spicedb.MapSpiceDBEndpoints();
    }

    /// <summary>
    /// Outputs healthcheck status in custom format
    /// </summary>
    /// <param name="context"></param>
    /// <param name="healthReport"></param>
    /// <returns></returns>
    internal static Task WriteHealthCheckResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            var entry = healthReport.Entries[VersionHealthCheck.NAME];
            foreach (var item in entry.Data)
            {
                jsonWriter.WritePropertyName(item.Key);

                JsonSerializer.Serialize(jsonWriter, item.Value,
                    item.Value?.GetType() ?? typeof(object));
            }
            foreach (var healthReportEntry in healthReport.Entries)
            {
                if (healthReportEntry.Key == VersionHealthCheck.NAME)
                {
                    // Skip the version one
                    continue;
                }
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("status",
                    healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("description",
                    healthReportEntry.Value.Description);

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    JsonSerializer.Serialize(jsonWriter, item.Value,
                        item.Value?.GetType() ?? typeof(object));
                }

                jsonWriter.WriteEndObject(); // end entry object
            }

            jsonWriter.WriteEndObject(); // end root
        }

        return context.Response.WriteAsync(
            Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}