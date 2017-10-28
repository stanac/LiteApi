using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace LiteApi.Tests.Fakes
{
    public class FakeHttpRequest : HttpRequest
    {
        private FakeHttpRequest() { }

        private HttpContext _httpContext;

        #region overrides

        public override Stream Body { get; set; }

        public override long? ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override IRequestCookieCollection Cookies { get; set; }

        public override IFormCollection Form { get; set; } = new FakeFormCollection();

        public override bool HasFormContentType { get; }

        public override IHeaderDictionary Headers { get; } = new FakeHeaderDictionary();

        public override HostString Host { get; set; }

        public override HttpContext HttpContext { get { return _httpContext; } }

        public override bool IsHttps { get; set; }

        public override string Method { get; set; }

        public override PathString Path { get; set; }

        public override PathString PathBase { get; set; }

        public override string Protocol { get; set; }

        public override IQueryCollection Query { get; set; } = new FakeQueryCollection(); 

        public override QueryString QueryString { get; set; }

        public override string Scheme { get; set; }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion

        public static FakeHttpRequest WithGetMethod()
        {
            return new FakeHttpRequest
            {
                Method = "GET"
            };
        }

        public static FakeHttpRequest WithPostMethod()
        {
            return new FakeHttpRequest
            {
                Method = "POST"
            };
        }

        public FakeHttpRequest WithPath(string path)
        {
            Path = path;
            return this;
        }

        public FakeHttpRequest AddQuery(string key, params string[] value)
        {
            (Query as FakeQueryCollection).Add(key, value);
            return this;
        }

        public FakeHttpRequest ClearQuery()
        {
            (Query as FakeQueryCollection).Clear();
            return this;
        }

        public FakeHttpRequest WriteBody(string body)
        {
            Body = new MemoryStream();
            var writer = new StreamWriter(Body);
            writer.Write(body);
            writer.Flush();
            Body.Position = 0;
            ContentLength = Body.Length;
            return this;
        }

        public void SetHttpCtx(HttpContext httpCtx)
        {
            _httpContext = httpCtx;
        }
    }
}
