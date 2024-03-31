using MKopa.Common.BrokerContracts;
using MKopa.Core.Entities.Http;
using MKopa.Core.Entities.Sms;

namespace MKopa.Core.Extensions
{
    public static class MapperExtensions
    {
        public static ISmsMessage ToSmsMessage(this IBrokerMessage brokerMessage)
        {
            if (brokerMessage == null) return default!;
            var smsMessage = new BaseSmsMessage()
            {
                Id = brokerMessage.Id,
                PhoneNumber = brokerMessage.PhoneNumber,
                SmsText = brokerMessage.SmsText,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = Entities.Enums.SmsMessageStatus.None,
                CountryCode = brokerMessage.CountryCode
            };

            return smsMessage;
        }

        public static IBrokerMessage ToBrokerMessage(this ISmsMessage smsMessage)
        {
            if (smsMessage == null) return default!;

            var brokerMessage = new BrokerMessage()
            {
                Id = smsMessage.Id,
                PhoneNumber = smsMessage.PhoneNumber,
                SmsText = smsMessage.SmsText,
                CountryCode = smsMessage.CountryCode
            };

            return brokerMessage;
        }

        public static IResponse ToLocalResponse(this HttpResponseMessage httpResponseMessage)
        {
            var response = new BaseResponse()
            {
                HttpResponseContent = httpResponseMessage.Content
            };

            return response;
        }

    }
}
