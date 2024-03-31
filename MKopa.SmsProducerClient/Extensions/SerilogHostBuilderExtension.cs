using Microsoft.Extensions.Hosting;
using Serilog;

namespace MKopa.SmsProducerClient.Extensions
{
    public static class SerilogHostBuilderExtension
    {
        public static IHostBuilder UseSerilogExtension(this IHostBuilder builder)
        {
            // TODO Improve configuration & Move to appsetting
            return builder.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File($"Logs/logreport-{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.txt", Serilog.Events.LogEventLevel.Warning));
        }
    }
}
