namespace MKopa.Core.Entities.Http
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        object HttpResponseContent { get; set; }
    }
}
