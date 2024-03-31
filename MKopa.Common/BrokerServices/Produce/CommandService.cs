using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MKopa.Common.BrokerContracts;
using MKopa.Common.Config;

namespace MKopa.Common.BrokerServices.Produce
{
    public class CommandService<T> : ICommandService where T : IBrokerMessage
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<CommandService<T>> _logger;
        private readonly IOptions<BusConfig> _options;

        public CommandService(ILogger<CommandService<T>> logger, IOptions<BusConfig> options, IBusControl busControl)
        {
            _logger = logger;
            _options = options;
            _busControl = busControl;
        }

        public async Task StopAsync()
        {
            await _busControl.StopAsync();
        }

        public async Task<bool> SendAsync(IBrokerMessage message)
        {
            try
            {
                if (_busControl != null)
                {
                    using var cancellationTokenSource = new CancellationTokenSource();
                    await _busControl.StartAsync(cancellationTokenSource.Token);
                    int counter = 1;
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        if (counter % 2 == 0)
                        { 
                            message.Id = Guid.NewGuid().ToString(); 
                            message.SmsText = $"Sms Text {counter}";
                        }

                        await _busControl.Publish(message, cancellationTokenSource.Token);
                        await Task.Delay(TimeSpan.FromSeconds(_options.Value.DelaySeconds));
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at method {nameof(SendAsync)} at {DateTime.Now.ToString()}: {ex.Message}");

            }
            finally
            {
                await _busControl.StopAsync();
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
                await _busControl.StopAsync();
            }

        }

    }
}
