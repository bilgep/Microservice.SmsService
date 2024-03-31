using Microsoft.EntityFrameworkCore;
using MKopa.DataAccess.DbContexts;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MKopa.DataAccess.Repository
{
    public class SmsMessageRepository : ISmsMessageRepository
    {
        private readonly SmsDbContext _context;
        private readonly ILogger<SmsMessageRepository> _logger;

        public SmsMessageRepository(SmsDbContext context, ILogger<SmsMessageRepository> logger)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _logger = logger;
        }

        /// <summary>
        /// Adds new ISmsMessage to database
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ISmsMessage?> AddSmsMessageAsync(ISmsMessage message)
        {
            try
            {
                if (message == null) throw new ArgumentNullException();

                var response = await _context.SmsMessages.AddAsync((BaseSmsMessage)message);
                _context.SaveChanges();
                if (response == null) throw new Exception();

                return await Task.FromResult(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(AddSmsMessageAsync)} method at {DateTime.UtcNow.ToString()}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets ISmsMessage from database
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ISmsMessage?> GetSmsMessageAsync(string messageId, string phoneNumber)
        { 
            try
            {
                if (messageId == null || phoneNumber == null) throw new ArgumentNullException();

                var response = await _context.SmsMessages.Where(m => m.Id.Equals(messageId) && m.PhoneNumber == phoneNumber).FirstOrDefaultAsync();

                if (response == null) _logger.LogError($"Sms message doesn't exist in the database- From: {nameof(GetSmsMessageAsync)} method at {DateTime.UtcNow.ToString()}");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(GetSmsMessageAsync)} method at {DateTime.UtcNow.ToString()}: {ex.Message}");
                throw;
            }

        }
        /// <summary>
        /// Gets registered Sms Provider Service with the highest priority from database
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ISmsProvider> GetPrimarySmsProviderByCountryAsync(string countryCode)
        {
            try
            {
                if(countryCode == null) throw new ArgumentNullException();

                var smsServiceProvider = await _context.SmsServiceProviders.Where(p => p.CountryCode == countryCode).Where(p => p.IsPrimary).FirstOrDefaultAsync();
                if(smsServiceProvider == null) throw new Exception();

                Type myType = default!;
                switch (countryCode)
                {
                    case "90":
                        myType = typeof(ProviderAProvider);
                        break;
                    default:
                        myType = typeof(BaseSmsProvider);
                        break;
                }

                var instantiatedObject = Activator.CreateInstance(myType) as ISmsProvider;
                if(instantiatedObject == null) throw new Exception();

                instantiatedObject.Id = smsServiceProvider.Id; 
                instantiatedObject.Name = smsServiceProvider.Name;
                instantiatedObject.IsPrimary = smsServiceProvider.IsPrimary;
                instantiatedObject.CountryCode = smsServiceProvider.CountryCode;

                return await Task.FromResult(instantiatedObject);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(GetPrimarySmsProviderByCountryAsync)} method at {DateTime.UtcNow.ToString()}: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// Updates ISmsMessage 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> UpdateSmsMessageAsync(ISmsMessage message)
        {
            try
            {
                if (message == null) throw new ArgumentNullException();

                _context.ChangeTracker.Clear();
                _context.SmsMessages.Update((BaseSmsMessage)message);
                var response = await _context.SaveChangesAsync();

                return response > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(UpdateSmsMessageAsync)} method at {DateTime.UtcNow.ToString()}: {ex.Message}");
                throw;
            }
            


        }
    }
}
