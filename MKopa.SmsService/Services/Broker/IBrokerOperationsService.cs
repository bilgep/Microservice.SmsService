using MKopa.Core.Entities.Sms;

namespace MKopa.Core.Services.Broker
{
    public interface IBrokerOperationsService
    {
        Task<bool> SendBrokerEventAsync(ISmsMessage smsMessage, IServiceProvider serviceProvider);
    }
}