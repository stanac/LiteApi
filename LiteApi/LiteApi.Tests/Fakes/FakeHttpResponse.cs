using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Moq;

namespace LiteApi.Tests.Fakes
{
    public class FakeHttpResponse : HttpResponse, IDisposable
    {
        internal IResponseCookies cookies = null;
        internal IHeaderDictionary headers = new FakeHeaderDictionary();
        internal HttpContext httpContext = null;
        internal bool hasStarted = false;

        public override Stream Body { get; set; } = new MemoryStream();

        public override long? ContentLength { get; set; }

        public override string ContentType
        {
            get => Headers.FirstOrDefault(x => x.Key == "Content-Type").Value;
            set => Headers["Content-Type"] = value;
        }

        public override IResponseCookies Cookies => cookies;

        public override bool HasStarted => hasStarted;

        public override IHeaderDictionary Headers => headers;

        public override HttpContext HttpContext => httpContext;

        public override int StatusCode { get; set; }

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            
        }

        public override void Redirect(string location, bool permanent)
        {
            
        }

        public void Dispose()
        {
            if (Body != null)
            {
                Body.Dispose();
            }
        }
    }
}
