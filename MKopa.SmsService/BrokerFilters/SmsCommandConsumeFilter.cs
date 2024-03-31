using FluentValidation;
using MassTransit;
using MKopa.Common.BrokerContracts;
using MKopa.Core.Services.General;


namespace MKopa.Common.BrokerServices.Consume
{
    public class SmsCommandConsumeFilter<T> : IFilter<ConsumeContext<T>>
        where T : class
    {
        private readonly ILogger<SmsCommandConsumeFilter<T>> _logger;

        private readonly IMessageProcessorService _messageProcessor;
        private readonly IValidator<BrokerMessage> _validator;

        public SmsCommandConsumeFilter(
            ILogger<SmsCommandConsumeFilter<T>> logger,
            IMessageProcessorService messageProcessor,
            IValidator<BrokerMessage> validator
            )
        {
            _logger = logger;
            _messageProcessor = messageProcessor;
            _validator = validator;
        }

        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            try
            {
                var messageReceived = (IBrokerMessage)context.Message;
                BrokerMessage brokerMessage = new() { CountryCode = messageReceived.CountryCode, Id = messageReceived.Id, PhoneNumber = messageReceived.PhoneNumber, SmsText = messageReceived.SmsText };
                var validationResult = _validator.Validate(brokerMessage);

                if (validationResult.IsValid)
                {
                    IServiceProvider serviceProvider = context.GetPayload<IServiceProvider>();

                    var response = await _messageProcessor.ProcessBrokerMessage(messageReceived);

                    if (!response) _logger.LogError($"Message processing failed - From: {nameof(SmsCommandConsumeFilter<T>)} at {DateTime.UtcNow.ToString()}");
                }
                else
                {
                    _logger.LogWarning($"Invalid message received at {nameof(SmsCommandConsumeFilter<T>)} at {DateTime.UtcNow.ToString()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured at {nameof(SmsCommandConsumeFilter<T>)} at {DateTime.UtcNow.ToString()}: {ex.Message}");
            }

            await next.Send(context);
        }
    }
}
