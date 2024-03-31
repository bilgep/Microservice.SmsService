namespace MKopa.Core.Config
{
    public interface IProviderConfig
    {
        string AcceptHeader { get; init; }
        string HttpClientName { get; init; }
        string SendSmsEndpointUri { get; init; }
        string UserAgent { get; init; }
        string HttpMethod { get; init; }
        string RequestUri { get; init; }
        string AuthenticationKey { get; init; }
        string Content {  get; init; }
        string Authorization { get; init; }
        string AccessToken { get; init; }
        string ContentType { get; init; }
        Dictionary<string,string> Headers { get; init; }

    }
}