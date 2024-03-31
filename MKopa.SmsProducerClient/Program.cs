
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MKopa.Common.BrokerContracts;
using MKopa.Common.BrokerServices.Produce;
using MKopa.SmsProducerClient;
using MKopa.SmsProducerClient.Extensions;
using System.Dynamic;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = CreateHost();

        // Send Command Messages to the Broker
        var commandService = ActivatorUtilities.CreateInstance<CommandService<IBrokerMessage>>(host.Services);


            var message = new BrokerMessage
            {
                Id = Guid.NewGuid().ToString(), 
                PhoneNumber = "05553332211",
                SmsText = $"Sms Text 1",
                CountryCode = "90"
            };

            await commandService.SendAsync(message);
    }

    private static IHost CreateHost() => Host.CreateDefaultBuilder()
    .UseServicesExtension()
    .UseMasstransitExtension()
    .UseSerilogExtension()
    .Build();
}