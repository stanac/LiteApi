using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace LiteApi.Tests.Fakes
{
    public class FakeApplicationBuilder : IApplicationBuilder
    {
        public bool CalledUse { get; private set; }

        public IServiceProvider ApplicationServices { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public IFeatureCollection ServerFeatures
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RequestDelegate Build()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder New()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            CalledUse = true;
            return this;
        }
    }
}
