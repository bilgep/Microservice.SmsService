using Castle.Core.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using MKopa.Core.Services.General;
using MKopa.Core.Services.Restful;
using MKopa.DataAccess.Repository;
using Moq;
using IRequest = MKopa.Core.Entities.Http.IRequest;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using MKopa.Core.Entities.Sms;
using MKopa.Common.BrokerContracts;
using MKopa.Core.Services.Database;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MKopa.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Http;
using MassTransit;
using MKopa.Core.Extensions;

namespace MKopa.SmsService.Tests
{
    public class SmsMessageDbServiceTests
    {
        private IBrokerMessage _brokerMessage;
        private DbContextOptions<SmsDbContext> _options;
        public SmsMessageDbServiceTests()
        {
            _brokerMessage = new BrokerMessage()
            {
                Id = "1",
                CountryCode = "90",
                PhoneNumber = "1234567890",
                SmsText = "Hello World",
            };

            _options = new DbContextOptionsBuilder<SmsDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemorySmsDb")
                .Options;


        }

        // TEST CASES
        // ShouldSaveMessageWithReceivedStatus
        // ShouldGetSmsProviderGivenTheCountryName
        // ShouldCheckIfMessageExistsInTheDatabase
        // ShouldUpdateMessageStatusWithGivenStatus

        [Fact]
        public void ShouldSaveMessageWithStatusOfReceived()
        {
            // Arrange
            var dbContext = new SmsDbContext(_options);

            var logger1 = new Mock<ILogger<SmsMessageRepository>>();
            var logger2 = new Mock<ILogger<SmsMessageDbService>>();
            var repository = new SmsMessageRepository(dbContext, logger1.Object);
            var smsDbService = new SmsMessageDbService(logger2.Object, repository);

            // Act
            var saveResponse = smsDbService.SaveMessageToDatabaseAsync(_brokerMessage);

            // Assert
            using (dbContext)
            {
                var response = dbContext.SmsMessages.FirstOrDefault(m => m.Id == _brokerMessage.Id && m.PhoneNumber == _brokerMessage.PhoneNumber);
                Assert.NotNull(response);
                Assert.Equal(SmsMessageStatus.Received, response.Status);
            }
        }

        [Fact]
        public void ShouldGetSmsProviderGivenTheCountryName()
        {
            
            using (var context = new SmsDbContext(_options))
            {
                // Arrange
                var logger1 = new Mock<ILogger<SmsMessageRepository>>();
                var logger2 = new Mock<ILogger<SmsMessageDbService>>();
                var repository = new SmsMessageRepository(context, logger1.Object);
                var smsDbService = new SmsMessageDbService(logger2.Object, repository);

                var providerA = new BaseSmsProvider { CountryCode = "90", Id = "1", IsPrimary = true, Name = "ProviderA" };
                context.SmsServiceProviders.Add(providerA);
                context.SmsServiceProviders.Add(new BaseSmsProvider { CountryCode = "90", Id = "2", IsPrimary = false, Name = "ProviderB" });
                context.SaveChanges();

                // Act
                var response = smsDbService.GetSmsProviderByCountryCodeAsync(_brokerMessage.CountryCode).Result;

                // Assert
                Assert.NotNull(response);
                Assert.Equal(providerA.Id, response.Id);

            }
        }

        [Fact]
        public void ShouldCheckIfMessageExistsInTheDatabase()
        {
            using (var context = new SmsDbContext(_options))
            {
                // Arrange
                var logger1 = new Mock<ILogger<SmsMessageRepository>>();
                var logger2 = new Mock<ILogger<SmsMessageDbService>>();
                var repository = new SmsMessageRepository(context, logger1.Object);
                var smsDbService = new SmsMessageDbService(logger2.Object, repository);
                context.SmsMessages.Add((BaseSmsMessage)_brokerMessage.ToSmsMessage());
                context.SaveChanges();

                // Act
                var response = smsDbService.SmsMessageExists(_brokerMessage.Id, _brokerMessage.PhoneNumber);

                // Assert
                Assert.NotNull(response);
                Assert.Equal(true, response.Result);

            }
        }

        [Fact]
        public void ShouldUpdateMessageStatusWithGivenStatus()
        {
            using (var context = new SmsDbContext(_options))
            {
                // Arrange
                var logger1 = new Mock<ILogger<SmsMessageRepository>>();
                var logger2 = new Mock<ILogger<SmsMessageDbService>>();
                var repository = new SmsMessageRepository(context, logger1.Object);
                var smsDbService = new SmsMessageDbService(logger2.Object, repository);
                var smsMessage = _brokerMessage.ToSmsMessage();
                smsMessage.Status = SmsMessageStatus.Sent;
                context.SmsMessages.Add((BaseSmsMessage)smsMessage);
                context.SaveChanges();


                // Act
                var response = smsDbService.UpdateSmsMessageStatusAsync(smsMessage, SmsMessageStatus.Sent);
                var responseObj = context.SmsMessages.Where(m => m.Id == smsMessage.Id).FirstOrDefaultAsync();
                // Assert
                Assert.NotNull(response);
                Assert.Equal(true, response.Result);
                Assert.Equal(smsMessage.Status, responseObj.Result!.Status);

            }
        }
    }
}