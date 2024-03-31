using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;

namespace MKopa.DataAccess.Repository
{
    public interface ISmsMessageRepository
    {
        Task<ISmsMessage?> GetSmsMessageAsync(string messageId, string phoneNumber);

        Task<ISmsMessage?> AddSmsMessageAsync(ISmsMessage message);

        Task<ISmsProvider?> GetPrimarySmsProviderByCountryAsync(string countryCode);

        Task<bool> UpdateSmsMessageAsync(ISmsMessage message);
    }
}
