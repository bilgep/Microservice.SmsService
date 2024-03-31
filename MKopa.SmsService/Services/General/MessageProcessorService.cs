
using MassTransit;
using MassTransit.Transports.Fabric;
using MKopa.Common.BrokerContracts;
using MKopa.Common.BrokerServices.Consume;
using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Sms;
using MKopa.Core.Extensions;
using MKopa.Core.Services.Broker;
using MKopa.Core.Services.Database;
using MKopa.Core.Services.Restful;
using static MKopa.Core.Extensions.ServicesHostBuilderExtension;

namespace MKopa.Core.Services.General
{
    public class MessageProcessorService : IMessageProcessorService
    {
        private readonly ILogger _logger;
        private readonly IBrokerMessage _brokerMessage;
        private readonly IBrokerOperationsService _brokerService;
        private readonly ISmsMessageDbService _smsMessageDbService;
        private readonly SmsServiceFactoryResolver _serviceFactoryResolver;
        private readonly IServiceProvider _serviceProvider;

        public MessageProcessorService(
            ILogger<MessageProcessorService> logger, 
            IBrokerMessage brokerMessage, 
            IBrokerOperationsService brokerService,
            ISmsMessageDbService smsMessageDbService,
            SmsServiceFactoryResolver serviceFactoryResolver,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _brokerMessage = brokerMessage;
            _brokerService = brokerService;
            _smsMessageDbService = smsMessageDbService;
            _serviceFactoryResolver = serviceFactoryResolver;
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> ProcessBrokerMessage(IBrokerMessage messageReceived)
        {
            try
            {
                // Check if message already received. Avoid duplication
                bool messageExists = await CheckIfMessageExistsInDb(messageReceived);
                if (messageExists) return false;

                // Save message in the database
                bool messageSaveResponse = await SaveReveivedBrokerMessageInDb(messageReceived);

                // Send message to the sms service provider
                var smsSenderProviderResponse = await SendSmsToSmsSenderProvider(messageReceived, _serviceFactoryResolver);

                if (!smsSenderProviderResponse)
                {
                    await UpdateFailedMessageStatus(messageReceived);
                    return false;
                }

                // Change Sms Message State to Sent
                bool sentMessageStatusUpdateResponse = await UpdateSentMessageStatus(messageReceived);

                // Send message to the broker when message sent to the sms service provider
                if (sentMessageStatusUpdateResponse)
                {
                    await SendEventMessageToBroker(messageReceived, _serviceProvider);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured at {nameof(MessageProcessorService)} at {DateTime.UtcNow.ToString()}: {ex.Message}");
            }

            return true;
        }


        private async Task SendEventMessageToBroker(IBrokerMessage _brokerMessage, IServiceProvider serviceProvider)
        {
            var brokerEventSendResponse = await _brokerService.SendBrokerEventAsync(_brokerMessage.ToSmsMessage(), serviceProvider);
            if (brokerEventSendResponse)
                _logger.LogInformation($"Sms sent event for the message with Id {_brokerMessage.Id} have been sent to the broker queue at {DateTime.Now.ToString()}");
            else _logger.LogError($"Error occured while sending broker event message with Id {_brokerMessage.Id} in the database at {DateTime.Now.ToString()}");
        }

        private async Task<bool> UpdateSentMessageStatus(IBrokerMessage _brokerMessage)
        {
            var updateResponse = await _smsMessageDbService.UpdateSmsMessageStatusAsync(_brokerMessage.ToSmsMessage(), SmsMessageStatus.Sent);
            if (updateResponse)
                _logger.LogInformation($"Message with Id {_brokerMessage.Id} saved in the database at {DateTime.Now.ToString()}");
            else _logger.LogError($"Error occured while saving the message with Id {_brokerMessage.Id} in the database at {DateTime.Now.ToString()}");
            return updateResponse;
        }

        private async Task UpdateFailedMessageStatus(IBrokerMessage _brokerMessage)
        {
            var response = await _smsMessageDbService.UpdateSmsMessageStatusAsync(_brokerMessage.ToSmsMessage(), SmsMessageStatus.Failed);
            if (response) _logger.LogInformation($"Status of the message with Id {_brokerMessage.Id} changed into failed status in the database at {DateTime.Now.ToString()}");
            else _logger.LogError($"Unable to change status of the message with Id {_brokerMessage.Id} into failed status in the database at {DateTime.Now.ToString()}");
        }

        private async Task<bool> SaveReveivedBrokerMessageInDb(IBrokerMessage _brokerMessage)
        {
            var response = await _smsMessageDbService.SaveMessageToDatabaseAsync(_brokerMessage);
            if (response)
            {
                _logger.LogInformation($"Message with Id {_brokerMessage.Id} saved in the database at {DateTime.Now.ToString()}");
            }
            else
            {
                _logger.LogError($"Error occured while saving the message with Id {_brokerMessage.Id} into the database at {DateTime.Now.ToString()}");
            }
            return response;
        }

        private async Task<bool> CheckIfMessageExistsInDb(IBrokerMessage _brokerMessage)
        {
            var response = await _smsMessageDbService.SmsMessageExists(_brokerMessage!.Id, _brokerMessage.PhoneNumber);
            if (response)
            {
                _logger.LogWarning($"Duplicate message with Id {_brokerMessage.Id} received at {DateTime.UtcNow.ToString()} ");
            }
            else
            {
                _logger.LogInformation($"New message with Id {_brokerMessage.Id} received at {DateTime.UtcNow.ToString()}");
            }
            return response;
        }

        private async Task<bool> SendSmsToSmsSenderProvider(IBrokerMessage _brokerMessage, SmsServiceFactoryResolver serviceFactoryResolver)
        {
            var smsMessage = _brokerMessage.ToSmsMessage();
            var provider = _smsMessageDbService.GetSmsProviderByCountryCode(smsMessage.CountryCode);
            ISmsSenderServiceFactory smsSenderServiceFactory = serviceFactoryResolver(smsMessage.CountryCode);
            var smsSenderResponse = await smsSenderServiceFactory.GetSmsSenderService().SendAsync((BaseSmsMessage)smsMessage);
            if (smsSenderResponse) _logger.LogInformation($"Sms message with Id {_brokerMessage.Id} sent to the third party sms service provider {provider.Name} at {DateTime.Now.ToString()}");
            return smsSenderResponse ? true : false;
        }



    }
}
