using System.Net.Http;

namespace Common
{
    public class ResponseObject
    {

        public object Data { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
    }
}