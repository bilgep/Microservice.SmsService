using Microsoft.Extensions.Hosting;

namespace MKopa.SmsProducerClient.Extensions
{
    public static class ServicesHostBuilderExtension
    {
        public static IHostBuilder UseServicesExtension(this IHostBuilder builder)
        {

            return builder.ConfigureServices((context, services) =>
            {   
            });
        }




    }
}
