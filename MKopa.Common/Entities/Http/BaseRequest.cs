
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using MKopa.Core.Config;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MKopa.Core.Entities.Http
{
    public class BaseRequest : IRequest
    {
        private readonly IOptions<BaseProviderConfig> _options;

        public BaseRequest(IOptions<BaseProviderConfig> options)
        {
            _options = options;
        }

        public virtual HttpRequestMessage CreateRequestMessage(ISmsMessage message)
        {
            var request = new HttpRequestMessage()
            {
                Method = new HttpMethod(_options.Value.HttpMethod.ToUpper()),
                Content = JsonSerializer.Deserialize<HttpContent>(JsonSerializer.Serialize<BaseSmsMessage>((BaseSmsMessage)message)),
                RequestUri = new Uri(_options.Value.RequestUri),
            };

            foreach (var header in _options.Value.Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            request.Headers.Authorization = new AuthenticationHeaderValue(_options.Value.AccessToken);
            return request;
        }
    }
}
