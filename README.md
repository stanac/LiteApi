# LiteApi

## MVC-style inspired .NET Core WEB API with JSON only support

[![Build status](https://ci.appveyor.com/api/projects/status/5crmsp0tfgduwcvo/branch/master?svg=true)] (https://ci.appveyor.com/project/stanac/liteapi/branch/master)
 (https://ci.appveyor.com/project/ProjectMona/liteapi) [![Coverage Status](https://img.shields.io/coveralls/stanac/LiteApi/master.svg?maxAge=900)](https://coveralls.io/github/stanac/LiteApi?&branch=master)

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
    // will respond to /api/test/add?a=3&b=8
    public int Add(int a, int b)
    {
        return a + b;
    }
}
``` 

More complex examples:

``` cs
[ControllerRoute("/api/v2/ops")]
public class OperationsController : LiteController
{
    // will respond to /api/v2/ops/3/plus/8
    [ActionRoute("/{a}/plus/{b}")]
    [HttpGet] // [HttpGet] is optional, by default it's GET, otherwise you can use [HttpPost], [HttpPut] or [HttpDelete]
    public int Add(int a, int b) => a + b;
    
    // will respond to /api/v2/ops/sum?ints=3&ints=6&ints=4
    public int Sum(IEnumerable<int> ints) => ints.Sum();
    
    // will respond to /api/v2/ops/join?a.1=one&a.3=three&b.2=two
    public IDictionary<int, string> Join(IDictionary<int, string> a, Dictionary<int, string> b)
    {
        Dictionary<int, string> c = new Dictionary<int, string>();
        foreach (var keyValue in a)
        {
            c[keyValue.Key] = keyValue.Value;
        }
        foreach (var keyValue in b)
        {
            c[keyValue.Key] = keyValue.Value;
        }
        return c;
    }

}
```

You can use attributes: HttpGetAttribute, HttpPostAttribute, HttpPutAttribute, 
HttpDeleteAttribute, by default action is GET if no attribute is set.

For more info check [Wiki](https://github.com/stanac/LiteApi/wiki/URL-mappings-to-controllers,-actions-and-parameters).

---

For installation you can reference nuget package `LiteApi` and use it in your startup class:

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

