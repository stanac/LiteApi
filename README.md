# LiteApi

## MVC-style inspired .NET Core WEB API with JSON only support

[![Build status](https://ci.appveyor.com/api/projects/status/5crmsp0tfgduwcvo/branch/master?svg=true)] (https://ci.appveyor.com/project/stanac/liteapi/branch/master)
 [![Coverage Status](https://img.shields.io/coveralls/stanac/LiteApi/master.svg?maxAge=900)](https://coveralls.io/github/stanac/LiteApi?&branch=master)
 [![NuGet](https://img.shields.io/nuget/v/LiteApi.svg)](https://www.nuget.org/packages/LiteApi)
 [![license](https://img.shields.io/github/license/stanac/liteapi.svg)]()

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

Restful controller:

``` cs
[RestfulLinks]
public class PersonsController: LiteController
{
    private readonly IPersonDataAccess _dataAccess;

    public PersonsController(IPersonDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    // method names are not affecting action routes

    [HttpGet] // will respond to /api/persons?id={someGuid}
    public PersonModel ById(Guid id) => _dataAccess.Get(id);

    [HttpGet, ActionRoute("/{id}")] // will respond to /api/persons/{someGuid}
    public PersonModel ByIdFromRoute([FromRoute]Guid id) => _dataAccess.Get(id);

    [HttpGet] // will respond to /api/persons
    public IEnumerable<PersonModel> All() => _dataAccess.GetAll();

    [HttpPost] // will respond to /api/persons
    public PersonModel Save(PersonModel model) => _dataAccess.Save(model);

    [HttpPost, ActionRoute("/{id}")] // will respond to /api/persons/{someGuid}
    public PersonModel Update(Guid id, PersonModel model) => _dataAccess.Update(id, model);
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

## Roadmap

### 1.3
- [ ] Support for other document formats (XML out of the box, extensibility points for other types)

### 1.2
- [ ] Performance testing, performance improvements with Roslyn code generators

### 1.1
- [ ] Support for generating OpenAPI documentation (Swagger)

### 1.0 Feature complete (stable)
- [ ] Stabilization, more testing

### 0.9 Feature complete (RC)
- [ ] Documentation and website

### 0.8
- [ ] Extensibility point to enable replacing any internal service in the middleware
- [ ] Support action parameters from request header

### 0.7
- [x] Support for file uploading (streams) [link](http://stanacev.com/2017/02/12/whats-new-in-liteapi-v0-7-0/#fileupload)
- [x] Support for file download (streams and byte arrays) [link](http://stanacev.com/2017/02/12/whats-new-in-liteapi-v0-7-0/#filedownload)
- [x] Support for action parameters from IServiceProvider (dependency injection on action level) [link](http://stanacev.com/2017/02/12/whats-new-in-liteapi-v0-7-0/#dependencyinjectiononactionlevel)

### Previous
- [x] Extensibility points for model binders and filters
- [x] Authorization [wiki](https://github.com/stanac/LiteApi/wiki/Authorization)
- [x] Restful links [link](http://stanacev.com/2017/02/12/whats-new-in-liteapi-v0-7-0/#restfullinks)
- [x] Actions overloading [link](http://stanacev.com/2017/02/12/whats-new-in-liteapi-v0-7-0/#actionoverloadingrules)
- [x] Dictionary from query
- [x] Collections from query
- [x] Default parameters
- [x] Proof of concept
