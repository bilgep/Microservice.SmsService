using MKopa.Core.Entities.Http;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;

namespace MKopa.Core.Services.Restful
{
    public interface ISmsSenderService
    {
        Task<bool> SendAsync(BaseSmsMessage message);

    }
}
