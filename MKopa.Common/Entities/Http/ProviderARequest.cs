
using MassTransit.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MKopa.Core.Config;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MKopa.Core.Entities.Http
{
    public class ProviderARequest : IRequest
    {
        private readonly BaseSmsMessage _smsMessage;
        private readonly IOptions<ProviderAConfig> _options;

        public ProviderARequest(ISmsMessage smsMessage, IOptions<ProviderAConfig> options)
        {
            _smsMessage = (BaseSmsMessage)smsMessage;
            _options = options;
        }

        public HttpRequestMessage CreateRequestMessage(ISmsMessage message)
        {
            var request = new HttpRequestMessage()
            {
                Method = new HttpMethod(_options.Value.HttpMethod.ToUpper()),
                RequestUri = new Uri(_options.Value.RequestUri),
            };

            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, _options.Value.ContentType);

            request.Content = content;

            if (_options.Value.Headers != null)
            {
                foreach (var header in _options.Value.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            // Apply additional ProviderA specific request messsage configuration 

            return request;
        }
    }
}
