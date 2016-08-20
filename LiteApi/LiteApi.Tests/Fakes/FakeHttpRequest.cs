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

        #region overrides

        public override Stream Body { get; set; }

        public override long? ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override IRequestCookieCollection Cookies { get; set; }

        public override IFormCollection Form { get; set; }

        public override bool HasFormContentType { get; }

        public override IHeaderDictionary Headers { get; }

        public override HostString Host { get; set; }

        public override HttpContext HttpContext { get; }

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

        public FakeHttpRequest WithPath(string path)
        {
            Path = path;
            return this;
        }

        public void AddQuery(string key, string value)
        {
            (Query as FakeQueryCollection).Add(key, value);
        }
    }
}
