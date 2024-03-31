using MKopa.Core.Entities.Http;
using MKopa.Core.Entities.Providers;

namespace MKopa.Core.Services.Restful
{
    public class BaseSmsSenderFactory : ISmsSenderServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IRequest _request;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseSmsSenderFactory(ILoggerFactory loggerFactory, IRequest request, IHttpClientFactory httpClientFactory)
        {
            _loggerFactory = loggerFactory;
            _request = request;
            _httpClientFactory = httpClientFactory;
        }

        public ISmsSenderService GetSmsSenderService()
        {
            return (ISmsSenderService)new BaseSmsSenderService(_loggerFactory, _request, _httpClientFactory);
        }
    }
}

