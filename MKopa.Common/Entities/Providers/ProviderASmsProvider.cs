namespace MKopa.Core.Entities.Providers
{
    public class ProviderASmsProvider : ISmsProvider
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
        public int PriorityLevel { get; set; } = default!;
        public bool IsPrimary { get; set; } = default!;
    }
}
