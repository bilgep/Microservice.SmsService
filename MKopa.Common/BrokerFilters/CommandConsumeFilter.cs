using MassTransit;
using Microsoft.Extensions.Logging;
using MKopa.Common.BrokerContracts;


namespace MKopa.Common.BrokerServices.Consume
{
    public class CommandConsumeFilter<T> : IFilter<ConsumeContext<T>>
        where T : class
    {
        private readonly ILogger<CommandConsumeFilter<T>> _logger;

        public CommandConsumeFilter(ILogger<CommandConsumeFilter<T>> logger)
        {
            _logger = logger;
        }

        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            try
            {
                var messageReceived = (IBrokerMessage)context.Message;
                _logger.LogInformation($"Message Received at {DateTime.UtcNow.ToString()}");
            }
            catch (Exception ex)
            {
                throw;
            }


            await next.Send(context);
        }
    }
}
