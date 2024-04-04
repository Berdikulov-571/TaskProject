using System.Net;

namespace TaskProject.Domain.Exceptions
{
    public class GlobalException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
        public string TitleMessage { get; set; } = default!;
    }
}