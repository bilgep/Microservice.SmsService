using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;

namespace MKopa.Core.Entities.Http
{
    public interface IRequest
    {
        HttpRequestMessage CreateRequestMessage(ISmsMessage message);
    }
}
