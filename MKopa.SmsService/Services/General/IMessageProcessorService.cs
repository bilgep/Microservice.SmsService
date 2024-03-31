using MKopa.Common.BrokerContracts;

namespace MKopa.Core.Services.General
{
    public interface IMessageProcessorService
    {
        Task<bool> ProcessBrokerMessage(IBrokerMessage messageReceived);
    }
}
