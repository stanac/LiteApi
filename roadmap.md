## Roadmap

### 1.4 (stable)
- [ ] Support for other document formats (XML out of the box, extensibility points for other types)

### 1.3 (stable)
- [ ] Support for versioning using request header value, versioning using routes is supported with `ControllerRouteAttribute`
- [ ] Enable multiple instances of the middleware to run in one app

### 1.2 (stable)
- [ ] Performance testing, performance improvements

### 1.1 (stable)
- [ ] Support for generating OpenAPI documentation (Swagger)

### 1.0 Feature complete (stable)
- [ ] Support for System.DateTimeOffset
- [ ] Choosing DateTime and DateTimeOffset format when parsing parameter values
- [ ] Automated integration tests

### 0.9 (stable)
- [x] Documentation and website ([liteapi.net](http://liteapi.net))
- [x] Global filters
- [x] Configuration to replace global API root route (replace "api" in URLs with custom string)
- [x] Support for both .NET Standard 1.6 and 2.0

### 0.8
- [x] Option to return raw JSON content from string and to set response code ([link](http://liteapi.net/docs/custom-response))
- [x] Extensibility point to enable replacing any internal service in the middleware ([link](http://liteapi.net/docs/replacing-internal-services))
- [x] Force HTTPS attribute in global config ([link](http://liteapi.net/docs/require-https))
- [x] Support action parameters from request header ([link](http://liteapi.net/docs/parameter-retrieving-from-header))

### 0.7
- [x] Support for file uploading (streams) ([link](http://liteapi.net/docs/files-upload-and-download))
- [x] Support for file download (streams and byte arrays) ([link](http://liteapi.net/docs/files-upload-and-download))
- [x] Support for action parameters from IServiceProvider (dependency injection on action level) ([link](http://liteapi.net/docs/parameter-retrieving-from-service-provider))

### Previous
- [x] Extensibility points for model binders and filters ([link](http://liteapi.net/docs/extensibility-points))
- [x] Authorization ([link](http://liteapi.net/docs/authorization))
- [x] Restful links ([link](http://liteapi.net/docs/action-matching))
- [x] Actions overloading ([link](http://liteapi.net/docs/action-overloading))
- [x] Dictionary from query ([link](http://liteapi.net/docs/parameters-retrieving-dictionaries))
- [x] Collections from query ([link](http://liteapi.net/docs/parameters-retrieving-collections))
- [x] Default parameters
- [x] Proof of concept
