using MKopa.Common.BrokerContracts;

namespace MKopa.Common.BrokerServices.Produce
{
    public interface IBrokerService
    {
        Task<bool> TryConnect();
        Task<bool> SendAsync(IBrokerMessage message);
    }
}
