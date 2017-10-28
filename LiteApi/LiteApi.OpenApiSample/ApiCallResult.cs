using System.ComponentModel.DataAnnotations;

namespace LiteApi.OpenApiSample
{
    public class ApiCallResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
