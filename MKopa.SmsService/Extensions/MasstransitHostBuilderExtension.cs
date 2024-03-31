using MassTransit;
using MKopa.Common.BrokerContracts;
using MKopa.Common.BrokerServices.Consume;
using MKopa.Common.BrokerServices.Produce;
using MKopa.Common.Config;

namespace MKopa.Core.Extensions
{
    public static class MasstransitHostBuilderExtension
    {
        public static WebApplicationBuilder UseMasstransitExtension(this WebApplicationBuilder builder)
        {
            var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq_BusControl");
            builder.Services.Configure<BusConfig>(rabbitMqConfig);
            var busConfig = rabbitMqConfig.Get<BusConfig>();

            if (busConfig!.IsCommandProducer || busConfig.IsEventProducer || busConfig.IsCommandConsumer)
            {
                builder.Services.AddMassTransit(x =>
                        x.UsingRabbitMq((IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator config) =>
                        {
                            config.Host(busConfig!.Host, busConfig.VirtualHost, h =>
                            {
                                h.Username(busConfig.UserName);
                                h.Password(busConfig.Password);
                            });

                            config.ConcurrentMessageLimit = 10;
                            config.UseMessageRetry(r => r.Interval(2,5));

                            if (busConfig.IsCommandConsumer)
                            {
                                config.ReceiveEndpoint(busConfig.SendCommandQueue, e =>
                                {
                                    e.Consumer<CommandConsumer>();
                                });

                                config.UseConsumeFilter(typeof(SmsCommandConsumeFilter<>), context);
                            }

                        }));

                builder.Services.AddSingleton<IHostedService, MassTransitHostedService>();
                builder.Services.AddTransient<IBrokerMessage, BrokerMessage>();


                if (busConfig.IsCommandProducer)
                    builder.Services.AddSingleton<ICommandService, CommandService<BrokerMessage>>();

                if (busConfig.IsEventProducer)
                    builder.Services.AddSingleton<IEventService , EventService<BrokerMessage>>();
 
            }

            return builder;
        }

    }
}
