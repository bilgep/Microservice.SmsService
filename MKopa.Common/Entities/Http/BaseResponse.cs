namespace MKopa.Core.Entities.Http
{
    public class BaseResponse : IResponse
    {
        public object HttpResponseContent { get; set; } = default!;

        public bool IsSuccess { get; set; } = default!;
    }
}
