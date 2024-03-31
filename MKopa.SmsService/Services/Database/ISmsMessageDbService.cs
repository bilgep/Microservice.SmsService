using MKopa.Common.BrokerContracts;
using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;

namespace MKopa.Core.Services.Database
{
    public interface ISmsMessageDbService
    {
        Task<bool> SmsMessageExists(string id, string phoneNumber);

        Task<ISmsProvider> GetSmsProviderByCountryCodeAsync(string countryCode);

         ISmsProvider GetSmsProviderByCountryCode(string countryCode);

        Task<bool> UpdateSmsMessageStatusAsync(ISmsMessage message, SmsMessageStatus messageStatus);

        Task<bool> SaveMessageToDatabaseAsync(IBrokerMessage messageReceived);

    }
}
