# LiteApi

## MVC-style inspired .NET Core WEB API with JSON only support

LiteApi is .net core framework inspired by MVC-style Controller/Action principles.
It's still in alpha and currently initial tests are showing that it can handle about 50% 
more requests per second than MVC6 (asp.net core).

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
HttpDeleteAttribute, buy default action is GET if no attribute is set.

---

For installation you need to build the framework yourself (no nugget release at the moment)
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
and currently framework does not support array types from query string. Plan is to
implement array support before initial stable release. Also a lot of testing needs to
be done before initial release.
