using MassTransit;
using MKopa.Common.BrokerContracts;

namespace MKopa.Common.BrokerServices.Consume
{
    public class CommandConsumer : IConsumer<IBrokerMessage>
    {
        public async Task Consume(ConsumeContext<IBrokerMessage> context)
        {

        }
    }
}
