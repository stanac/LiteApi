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
        public override AuthenticationManager Authentication { get; }

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
