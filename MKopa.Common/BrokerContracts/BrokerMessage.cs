namespace MKopa.Common.BrokerContracts
{
    public record BrokerMessage : IBrokerMessage
    {
        public string Id { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string SmsText { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
    }
}
