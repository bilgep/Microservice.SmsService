namespace MKopa.Core.Config
{
    public class BaseProviderConfig : IProviderConfig
    {
        public string HttpClientName { get; init; } = default!;
        public string SendSmsEndpointUri { get; init; } = default!;
        public string Accept { get; init; } = default!;
        public string UserAgent { get; init; } = default!;
        public string AcceptHeader { get; init; } = default!;
        public string HttpMethod { get; init; } = default!;
        public string RequestUri { get; init; } = default!;
        public string AuthenticationKey { get; init; } = default!;
        public string Content { get; init; } = default!;
        public string Authorization { get; init; } = default!;
        public string AccessToken { get; init; } = default!;
        public string ContentType { get; init; } = default!;
        public Dictionary<string, string> Headers { get; init; } = default!;
    }

}