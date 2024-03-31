

using Microsoft.Extensions.DependencyInjection;
using MKopa.Common.BrokerServices.Produce;
using MKopa.Core.Config;
using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Http;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using MKopa.DataAccess.Repository;
using MKopa.Core.Services.Broker;
using MKopa.Core.Services.Database;
using MKopa.Core.Services.General;
using MKopa.Core.Services.Restful;
using FluentValidation.AspNetCore;
using FluentValidation;
using MKopa.Common.BrokerContracts;
using MKopa.SmsService.Validators;

namespace MKopa.Core.Extensions
{
    public static class ServicesHostBuilderExtension
    {
        public delegate ISmsSenderServiceFactory SmsServiceFactoryResolver(string countryCode);
        public static WebApplicationBuilder UseServicesExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<BrokerMessage>, BrokerMessageValidator>();

            builder.Services.Configure<BaseProviderConfig>(builder.Configuration.GetSection("BaseSmsServiceProvider"));
            builder.Services.Configure<ProviderAConfig>(builder.Configuration.GetSection("SmsServiceProviderA"));

            builder.Services.AddScoped<ISmsMessage, BaseSmsMessage>();
            builder.Services.AddScoped<IRequest, BaseRequest>();
            builder.Services.AddScoped<ProviderARequest>();


            builder.Services.AddScoped<ISmsMessageRepository, SmsMessageRepository>();
            builder.Services.AddScoped<ISmsMessageDbService , SmsMessageDbService>();

            builder.Services.AddScoped<ISmsSenderServiceFactory, BaseSmsSenderFactory>();
            builder.Services.AddScoped<ProviderASmsSenderFactory>();

            builder.Services.AddScoped<ISmsSenderService, BaseSmsSenderService>();
            builder.Services.AddScoped<ProviderASmsSenderService>();

            builder.Services.AddScoped<IBrokerOperationsService, BrokerOperationsService>();


            builder.Services.AddScoped<ISmsProvider, BaseSmsProvider>();
            builder.Services.AddScoped<ProviderAProvider>();

            builder.Services.AddScoped<IMessageProcessorService, MessageProcessorService>();

            builder.Services.AddTransient<SmsServiceFactoryResolver>(serviceProvider => countryCode =>
            {
                switch (countryCode)
                {
                    case "90":
                        return serviceProvider.GetService<ProviderASmsSenderFactory>()!;
                    default:
                        return serviceProvider.GetService<BaseSmsSenderFactory>()!;
                }
            });

            builder.Services.AddHttpClient();
            return builder;
        }




    }
}
