using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using MKopa.DataAccess.DbContexts;
using MKopa.DataAccess.Repository;
using Moq;

namespace MKopa.DataAccess.Tests
{
    public class SmsMessageRepositoryTests
    {
        private DbContextOptions<SmsDbContext>  _options;
        private readonly BaseSmsMessage _smsMessage;
        public SmsMessageRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<SmsDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemorySmsDb")
                .Options;

            _smsMessage = new BaseSmsMessage
            {
                Id = "1",
                CountryCode = "00",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                PhoneNumber = "1234567890",
                SmsText = "Hello World",
                Status = Core.Entities.Enums.SmsMessageStatus.None
            };
        }

        // Test Cases
            // ShouldGetAddSmsMessage
            // ShouldGetSmsMessage
            // ShouldGetPrimarySmsProvider
            // ShouldUpdateSmsMessage

        [Fact]
        public void ShouldAddSmsMessage()
        {
            // Arrange
            var logger = new Mock<ILogger<SmsMessageRepository>>();


            // Act
            using (var context = new SmsDbContext(_options))
            {
                var repository = new SmsMessageRepository(context, logger.Object);
                repository.AddSmsMessageAsync(_smsMessage).Wait();
            }

            // Assert
            using (var context = new SmsDbContext(_options))
            {
                var smsMessages = context.SmsMessages.ToList();
                var savedMessage = Assert.Single(smsMessages);

                Assert.Equal(_smsMessage.Id, savedMessage.Id);
                Assert.Equal(_smsMessage.CountryCode, savedMessage.CountryCode);
                Assert.Equal(_smsMessage.CreatedDate, savedMessage.CreatedDate);
                Assert.Equal(_smsMessage.ModifiedDate, savedMessage.ModifiedDate);
                Assert.Equal(_smsMessage.PhoneNumber, savedMessage.PhoneNumber);
                Assert.Equal(_smsMessage.SmsText, savedMessage.SmsText);
                Assert.Equal(_smsMessage.Status, savedMessage.Status);
            }
        }

        [Fact]
        public void ShouldGetSmsMessage()
        {
            // Arrange
            var logger = new Mock<ILogger<SmsMessageRepository>>();
            using (var context = new SmsDbContext(_options))
            {
                context.SmsMessages.Add(_smsMessage);
                context.SaveChanges();
            }

            // Act
            using (var context = new SmsDbContext(_options))
            {

                var repository = new SmsMessageRepository(context, logger.Object);
                repository.GetSmsMessageAsync(_smsMessage.Id, _smsMessage.PhoneNumber).Wait();
            }

            // Assert
            using (var context = new SmsDbContext(_options))
            {
                var smsMessages = context.SmsMessages.ToList();
                var savedMessage = Assert.Single(smsMessages);

                Assert.Equal(_smsMessage.Id, savedMessage.Id);
                Assert.Equal(_smsMessage.CountryCode, savedMessage.CountryCode);
                Assert.Equal(_smsMessage.CreatedDate, savedMessage.CreatedDate);
                Assert.Equal(_smsMessage.ModifiedDate, savedMessage.ModifiedDate);
                Assert.Equal(_smsMessage.PhoneNumber, savedMessage.PhoneNumber);
                Assert.Equal(_smsMessage.SmsText, savedMessage.SmsText);
                Assert.Equal(_smsMessage.Status, savedMessage.Status);
            }
        }

        [Fact]
        public void ShouldGetPrimarySmsProvider()
        {
            // Arrange
            var logger = new Mock<ILogger<SmsMessageRepository>>();
            List<BaseSmsProvider> providers = new List<BaseSmsProvider>();
            using (var context = new SmsDbContext(_options))
            {
                providers = new List<BaseSmsProvider>
                {
                    new BaseSmsProvider { Id = "1", IsPrimary = true, CountryCode = "00", Name = "BaseProviderA" },
                    new BaseSmsProvider { Id = "2", IsPrimary = false, CountryCode = "00", Name = "BaseProviderB" },
                    new BaseSmsProvider { Id = "3", IsPrimary = false, CountryCode = "00", Name = "BaseProviderC" }
                };

                context.SmsServiceProviders.AddRange(providers);
                context.SaveChanges();
            }

            // Act
            BaseSmsProvider primaryProvider;
            using (var context = new SmsDbContext(_options))
            {
                var repository = new SmsMessageRepository(context, logger.Object);
                primaryProvider = (BaseSmsProvider)repository.GetPrimarySmsProviderByCountryAsync(_smsMessage.CountryCode).Result;
            }

            // Assert
            using (var context = new SmsDbContext(_options))
            {
                Assert.NotNull(primaryProvider);
                Assert.Equal(providers[0].Id, primaryProvider.Id);
            }
        }

        [Fact]
        public async void ShouldUpdateSmsMessage()
        {
            // Arrange
            var logger = new Mock<ILogger<SmsMessageRepository>>();
            var modifiedSmsMessage = new BaseSmsMessage();
            modifiedSmsMessage = _smsMessage;
            modifiedSmsMessage.Status = Core.Entities.Enums.SmsMessageStatus.Failed;
            using (var context = new SmsDbContext(_options))
            {
                var repository = new SmsMessageRepository(context, logger.Object);
                repository.AddSmsMessageAsync(modifiedSmsMessage).Wait();
            }

            // Act
            BaseSmsMessage retrievedSmsMessage = new();
            using (var context = new SmsDbContext(_options))
            {
                var repository = new SmsMessageRepository(context, logger.Object);
                repository.UpdateSmsMessageAsync(modifiedSmsMessage).Wait();
                retrievedSmsMessage = (BaseSmsMessage)repository.GetSmsMessageAsync(modifiedSmsMessage.Id, modifiedSmsMessage.PhoneNumber).Result;
            }

            // Assert
            using (var context = new SmsDbContext(_options))
            {
                Assert.NotNull(retrievedSmsMessage);
                Assert.Equal(retrievedSmsMessage.Status, modifiedSmsMessage.Status);
            }
        }

    }
}