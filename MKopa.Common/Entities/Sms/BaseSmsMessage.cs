using MKopa.Core.Entities.Enums;

namespace MKopa.Core.Entities.Sms
{
    public class BaseSmsMessage : ISmsMessage
    {
        public string Id { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string SmsText { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ModifiedDate { get; set; } = DateTimeOffset.Now;
        public SmsMessageStatus Status { get; set; } = SmsMessageStatus.None;

    }

    public static class BaseSmsMessageExtensions
    {
        public static BaseSmsMessage Initialize(this BaseSmsMessage message)
        {
            message = new BaseSmsMessage 
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "555555555",
                SmsText = "Hello world",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                ModifiedDate = DateTime.UtcNow.AddMinutes(-5),
                Status = SmsMessageStatus.Received,
                CountryCode = CountryCode.Turkiye,
            };
            return message;
        }
    }
}
