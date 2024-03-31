namespace MKopa.Core.Entities.Providers
{
    public interface ISmsProvider
    {
        string CountryCode { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        bool IsPrimary { get; set; }
    }
}