using MKopa.Core.Entities.Enums;

namespace MKopa.Core.Entities.Sms
{
    public interface ISmsMessage
    {
        DateTimeOffset? CreatedDate { get; set; }
        string Id { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }
        string PhoneNumber { get; set; }
        string SmsText { get; set; }
        string CountryCode { get; set; }
        SmsMessageStatus Status { get; set; }
    }
}