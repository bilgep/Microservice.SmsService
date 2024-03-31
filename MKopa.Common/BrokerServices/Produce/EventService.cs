using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MKopa.Common.BrokerContracts;
using MKopa.Common.Config;

namespace MKopa.Common.BrokerServices.Produce
{
    public class EventService<T> : IEventService where T : IBrokerMessage
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<CommandService<T>> _logger;
        private readonly IOptions<BusConfig> _options;

        public EventService(ILogger<CommandService<T>> logger, IOptions<BusConfig> options, IBusControl busControl)
        {
            _logger = logger;
            _options = options;
            _busControl = busControl;
        }

        public async Task<bool> SendAsync(IBrokerMessage message)
        {
            try
            {
                if (_busControl != null)
                {
                    var sendToUri = new Uri(_options.Value.SmsSentEventQueueUri);
                    var endpoint = await _busControl.GetSendEndpoint(sendToUri);

                    using var cancellationTokenSource = new CancellationTokenSource();
                    //await _busControl.StartAsync(cancellationTokenSource.Token);


                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        await endpoint.Send(message, cancellationTokenSource.Token);
                        _logger.LogInformation($"Sms sent event for the message with Id {message.Id} have been sent to the broker queue at {DateTime.Now.ToString()}");
                        await Task.Delay(TimeSpan.FromSeconds(_options.Value.DelaySeconds));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at method {nameof(SendAsync)} at {DateTime.Now.ToString()}: {ex.Message}");

            }
            finally
            {
                //await _busControl.StopAsync();
                await Task.CompletedTask;
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> TryConnect()
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource();
                var busHandle = await _busControl.StartAsync(cancellationTokenSource.Token);
                return busHandle.Ready.Status == TaskStatus.RanToCompletion ? true : false;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at method {nameof(TryConnect)} at {DateTime.Now.ToString()}: {ex.Message}");
                return false;
            }
            finally
            {
                //await _busControl.StopAsync();
            }
        }
    }
}
