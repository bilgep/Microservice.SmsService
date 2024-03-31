using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MKopa.Common.BrokerContracts;
using MKopa.Common.BrokerServices.Consume;
using MKopa.Common.BrokerServices.Produce;
using MKopa.Common.Config;


namespace MKopa.SmsProducerClient.Extensions
{
    public static class MasstransitHostBuilderExtension
    {
        public static IHostBuilder UseMasstransitExtension(this IHostBuilder builder)
        {

            return builder.ConfigureServices((context, services) =>
            {

                var rabbitMqConfig = context.Configuration.GetSection("RabbitMq_BusControl");
                services.Configure<BusConfig>(rabbitMqConfig);
                var busConfig = rabbitMqConfig.Get<BusConfig>();

                if (busConfig!.IsCommandProducer || busConfig.IsEventProducer || busConfig.IsCommandConsumer)
                {
                    services.AddMassTransit(x =>
                        x.UsingRabbitMq((context, config) =>
                        {
                            config.Host(busConfig!.Host, busConfig.VirtualHost, h =>
                            {
                                h.Username(busConfig.UserName);
                                h.Password(busConfig.Password);
                            });

                            config.UseMessageRetry(r => r.Interval(2, 5));

                            if (busConfig.IsCommandConsumer)
                            {
                                config.ReceiveEndpoint(busConfig.SendCommandQueue, e =>
                                {
                                    e.Consumer<CommandConsumer>();
                                });

                                config.UseConsumeFilter(typeof(CommandConsumeFilter<>), context);
                            }

                        }));

                    services.AddSingleton<IHostedService, MassTransitHostedService>();
                    services.AddTransient<IBrokerMessage, BrokerMessage>();

                    if (busConfig.IsCommandProducer)
                        services.AddTransient<ICommandService, CommandService<BrokerMessage>>();

                    if (busConfig.IsEventProducer)
                        services.AddTransient<EventService<BrokerMessage>>();
                }


            });
        }

    }
}
