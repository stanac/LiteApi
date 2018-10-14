using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using System.Threading;

namespace LiteApi.Tests.Fakes
{
    public class FakeHttpContext : HttpContext
    {
        public FakeHttpContext()
        {
            (Request as FakeHttpRequest).SetHttpCtx(this);
            this.SetLiteApiOptions(LiteApiOptions.Default);
        }

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0672 // Member overrides obsolete member
        public override AuthenticationManager Authentication { get; }
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore CS0618 // Type or member is obsolete

        public override ConnectionInfo Connection { get; }

        public override IFeatureCollection Features { get; }

        public override IDictionary<object, object> Items { get; set; } = new Dictionary<object, object>();

        public override HttpRequest Request { get; } = FakeHttpRequest.WithGetMethod();

        public override CancellationToken RequestAborted { get; set; }

        public override IServiceProvider RequestServices { get; set; }

        public override HttpResponse Response { get; } = new FakeHttpResponse();

        public override ISession Session { get; set; }

        public override string TraceIdentifier { get; set; }

        public override ClaimsPrincipal User { get; set; }

        public override WebSocketManager WebSockets { get; }

        public override void Abort()
        {
            
        }
    }
}
