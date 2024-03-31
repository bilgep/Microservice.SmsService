﻿using MassTransit;
using MassTransit.JobService;
using MKopa.Core.Entities.Http;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using MKopa.Core.Extensions;
using MKopa.Core.Services.Database;
using MKopa.Core.Services.General;
using System.Net.Http;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace MKopa.Core.Services.Restful
{
    public class ProviderASmsSenderService : ISmsSenderService
    {
        private readonly ILogger<ProviderASmsSenderService> _logger;
        private readonly IRequest _request;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProviderASmsSenderService(ILoggerFactory loggerFactory, IRequest request, IHttpClientFactory httpClientFactory)
        {
            _logger = loggerFactory.CreateLogger<ProviderASmsSenderService>();
            _request = request;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> SendAsync(BaseSmsMessage message)
        {
            try
            {
                //var httpRequestMessage = _request.CreateRequestMessage(message);
                //var httpClient = _httpClientFactory.CreateClient();
                //var response = await httpClient.PostAsync(httpRequestMessage.RequestUri, httpRequestMessage.Content);
                //var result = await response.Content.ReadAsStringAsync();
                //response.EnsureSuccessStatusCode();
                //// Do ProviderA specific additional operations
                //if (response.IsSuccessStatusCode)
                //{ 
                //    _logger.LogInformation($"SMS message with id {message.Id} sent to the sms service provider at {DateTime.UtcNow.ToString()}"); 
                //    return true;
                //}
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured at {nameof(ProviderASmsSenderService)} at {DateTime.UtcNow.ToString()}: {ex.Message}");
            }
            return false;
        }
    }
}
