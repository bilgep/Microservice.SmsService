using MKopa.Core.Entities.Http;
using MKopa.Core.Entities.Providers;

namespace MKopa.Core.Services.Restful
{
    public class ProviderASmsSenderFactory : ISmsSenderServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ProviderARequest _request;
        private readonly IHttpClientFactory _httpClientFactory;


        public ProviderASmsSenderFactory(ILoggerFactory loggerFactory, ProviderARequest request, IHttpClientFactory httpClientFactory)
        {
            _loggerFactory = loggerFactory;
            _request = request;
            _httpClientFactory = httpClientFactory;
        }

        public ISmsSenderService GetSmsSenderService()
        {
            var service = new ProviderASmsSenderService(_loggerFactory, _request, _httpClientFactory);
            return (ISmsSenderService)service;
        }
    }

}

