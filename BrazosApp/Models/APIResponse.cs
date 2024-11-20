using System.Net;

namespace BrazosApp.Models
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
