using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Providers;

namespace MKopa.Core.Entities.Providers
{
    public class ProviderAProvider : ISmsProvider
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
        public bool IsPrimary { get; set; } = default!;
    }
}
