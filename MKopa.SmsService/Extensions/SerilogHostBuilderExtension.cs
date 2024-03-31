
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MKopa.DataAccess.DbContexts;
using Serilog;

namespace MKopa.Core.Extensions
{
    public static class SerilogHostBuilderExtension
    {
        public static WebApplicationBuilder UseSerilogExtension(this WebApplicationBuilder builder)
        {
            // TODO Improve configuration
            builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File($"Logs/logreport-{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.txt", Serilog.Events.LogEventLevel.Warning));

            return builder;
        }
    }
}
