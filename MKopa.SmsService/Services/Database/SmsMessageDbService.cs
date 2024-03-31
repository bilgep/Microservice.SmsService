using MKopa.Common.BrokerContracts;
using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using MKopa.Core.Extensions;
using MKopa.DataAccess.Repository;

namespace MKopa.Core.Services.Database
{
    public class SmsMessageDbService : ISmsMessageDbService
    {
        private readonly ILogger<SmsMessageDbService> _logger;
        private readonly ISmsMessageRepository _repository;

        public SmsMessageDbService(ILogger<SmsMessageDbService> logger, ISmsMessageRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> SaveMessageToDatabaseAsync(IBrokerMessage message)
        {
            var domainSmsMessage = message.ToSmsMessage();
            domainSmsMessage.Status = SmsMessageStatus.Received;
            var response = await _repository.AddSmsMessageAsync(domainSmsMessage);
            if (response != null) _logger.LogInformation($"New message with Id {message.Id} saved in the database at {DateTime.UtcNow.ToString()}");
            return response != null ? true : false;
        }

        public async Task<ISmsProvider> GetSmsProviderByCountryCodeAsync(string countryCode)
        {
            var smsProvider = await _repository.GetPrimarySmsProviderByCountryAsync(countryCode);
            return smsProvider;
        }

        public async Task<bool> SmsMessageExists(string id, string phoneNumber)
        {
            try
            {
                var smsMessage = await _repository.GetSmsMessageAsync(id, phoneNumber);
                return smsMessage == null ? false : true;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<bool> UpdateSmsMessageStatusAsync(ISmsMessage message, SmsMessageStatus messageStatus)
        {
            message.Status = messageStatus;
            var response = await _repository.UpdateSmsMessageAsync(message);
            return response ? true : false;
        }

        public ISmsProvider GetSmsProviderByCountryCode(string countryCode)
        {
            var smsProvider = _repository.GetPrimarySmsProviderByCountryAsync(countryCode);
            return smsProvider.Result;
        }

    }
}
