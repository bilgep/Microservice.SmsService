namespace MKopa.Common.BrokerServices.Produce
{
    public interface ICommandService : IBrokerService
    {
        Task StopAsync();

    }
}
