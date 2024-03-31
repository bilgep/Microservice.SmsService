using MKopa.Common.BrokerServices.Produce;
using MKopa.Core.Entities.Sms;
using MKopa.Core.Extensions;

namespace MKopa.Core.Services.Broker
{
    public class BrokerOperationsService : IBrokerOperationsService
    {
        public async Task<bool> SendBrokerEventAsync(ISmsMessage smsMessage, IServiceProvider serviceProvider)
        {
            var brokerEventService = serviceProvider.GetRequiredService<IEventService>();
            var brokerEventSentResponse = await brokerEventService.SendAsync(smsMessage.ToBrokerMessage());
            return brokerEventSentResponse ? true : false;
        }
    }
}
