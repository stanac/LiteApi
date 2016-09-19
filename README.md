# LiteApi

## MVC-style inspired .NET Core WEB API with JSON only support

[![Build status](https://ci.appveyor.com/api/projects/status/5crmsp0tfgduwcvo?svg=true)](https://ci.appveyor.com/project/ProjectMona/liteapi) [![Coverage Status](https://coveralls.io/repos/github/stanac/LiteApi/badge.svg?branch=master)](https://coveralls.io/github/stanac/LiteApi?branch=master)

LiteApi is .net core middleware inspired by MVC-style Controller/Action principles.
It's still in beta and currently initial tests are showing that it can handle about 50%-90%
more requests per second than MVC6 (asp.net core). Check [intro post](http://stanacev.com/2016/09/06/liteapi-alternative-web-api-net-core-middleware-intro/) and [performance comparison post](http://stanacev.com/2016/09/08/liteapi-performance-comparison/).

LiteApi is still in prerelease phase, to install it use nuget, or add it to project.json file manually:

```
Install-Package LiteApi -Pre
```

Super simple example of a controller: 

``` cs
public class TestController : LiteController
{
    // will respond to protocol://host:port/api/test/add?a=3&b=8
    public int Add(int a, int b)
    {
        return a + b;
    }
}
``` 

You can use attributes: HttpGetAttribute, HttpPostAttribute, HttpPutAttribute, 
HttpDeleteAttribute, by default action is GET if no attribute is set.

---

For installation you need to build the middleware yourself (no nuget release at the moment)
and use middleware in your startup class:

``` cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LiteApi;

namespace MyApp
{
    public class Startup
    {
        // ... configure services ...
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseStaticFiles(); // if you want static files (LiteApi does not support static files by itself)
            app.UseLiteApi();
        }
    }
}
```

---

There is a lot of documentation to be written about parameters and actions matching,
also a lot of testing needs to be done before initial release.
