namespace MKopa.Common.BrokerContracts
{
    public interface IBrokerMessage
    {
        string Id { get; set; }
        string PhoneNumber { get; set; }
        string SmsText { get; set; }
        string CountryCode { get; set; }
    }
}
