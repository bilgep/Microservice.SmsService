using Microsoft.Net.Http.Headers;
using MKopa.Core.Config;
using MKopa.DataAccess.Repository;

namespace MKopa.Core.Extensions
{
    public static class HttpClientBuilderExtension
    {
        public static WebApplicationBuilder UseConfiguredHttpClient(this WebApplicationBuilder builder)
        {

            var smsServiceProviderConfig = builder.Configuration.GetSection("SmsServiceProviderA");
            builder.Services.Configure<BaseProviderConfig>(smsServiceProviderConfig);
            var serviceProviderConfig = smsServiceProviderConfig.Get<BaseProviderConfig>();

            builder.Services.AddHttpClient(serviceProviderConfig!.HttpClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(serviceProviderConfig!.SendSmsEndpointUri);
                httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, serviceProviderConfig.UserAgent);
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, serviceProviderConfig.Accept);
            });

            builder.Services.AddScoped<ISmsMessageRepository, SmsMessageRepository>();
            builder.Services.AddScoped<IProviderConfig, BaseProviderConfig>();
            builder.Services.AddScoped<ProviderAConfig>();
            return builder;
        }
    }
}
